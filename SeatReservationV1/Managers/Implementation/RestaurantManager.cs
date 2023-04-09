using AutoMapper;
using SeatReservationCore.Extensions;
using SeatReservationCore.Helpers;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Microservices.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Options;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;
using System.Drawing;
using System.Drawing.Imaging;

namespace SeatReservationV1.Managers.Implementation
{
    public class RestaurantManager : IRestaurantManager
    {
        private readonly IMapper _mapper;

        private readonly AppSettings _appSettings;

        private readonly ISimilarImagesMicroservice _similarImagesMicroservice;

        private readonly RestaurantsRepository _restaurantsRepository;
        private readonly ImagesToRestaurantsRepository _imagesToRestaurantsRepository;
        private readonly FavoritesRestaurantsRepository _favoritesRestaurantsRepository;

        public RestaurantManager(IMapper mapper,
            AppSettings appSettings,
            ISimilarImagesMicroservice similarImagesMicroservice,
            RestaurantsRepository restaurantsRepository,
            ImagesToRestaurantsRepository imagesToRestaurantsRepository,
            FavoritesRestaurantsRepository favoritesRestaurantsRepository)
        {
            _mapper = mapper;
            _appSettings = appSettings;
            _similarImagesMicroservice = similarImagesMicroservice;
            _restaurantsRepository = restaurantsRepository;
            _imagesToRestaurantsRepository = imagesToRestaurantsRepository;
            _favoritesRestaurantsRepository = favoritesRestaurantsRepository;
        }

        public async Task<int> CreateAsync(CreateRestaurantVM createModel)
        {
            var restaurantEntity = _mapper.Map<RestaurantEntity>(createModel);

            var restaurantId = await _restaurantsRepository.CreateAsync(restaurantEntity);
            
            if (createModel.ImageIds.HasElement())
            {
                await _imagesToRestaurantsRepository.CreateAsync(createModel.ImageIds.Select(imageId => new ImageToRestaurantEntity 
                { 
                    RestaurantId = restaurantId,
                    ImageId = imageId,
                    IsActive = true
                }));

                var imageIdsToContents = await _imagesToRestaurantsRepository.GetImageIdsToContentsAsync(createModel.ImageIds);

                foreach (var item in imageIdsToContents)
                {
                    //TODO сохраняем локально для обработки изображений ИИ(пока не реализовывал другую логику)
                    var localName = $"{item.Key}.jpg";
                    var fileSavePath = Path.Combine(_appSettings.ImageSaveDir, localName);

                    MemoryStream memoryStream = new MemoryStream(item.Value);
                    Image image = Image.FromStream(memoryStream);

                    image.Save(fileSavePath, ImageFormat.Jpeg);
                }

                _similarImagesMicroservice.IndexingAsync();
            }

            return restaurantId;
        }

        public async Task<IEnumerable<RestaurantVM>> GetByFilterAsync(RestaurantsFilterVM filter)
        {
            IEnumerable<RestaurantEntity> restaurants = null;

            if (filter.ImageId > 0)
            {
                var imageIds = await _similarImagesMicroservice.SearchAsync(filter.ImageId.Value);
                if (!imageIds.HasElement())
                    return Enumerable.Empty<RestaurantVM>();

                var restaurantIdsByImages = await _imagesToRestaurantsRepository.GetRestaurantsByImagesAsync(imageIds);

                restaurants = await _restaurantsRepository.GetByIdsAsync(restaurantIdsByImages);
            }

            restaurants = filter.ImageId > 0
                ? restaurants
                : await _restaurantsRepository.GetByFilterAsync(filter.Take, filter.Skip, filter.Filter);

            return await ToRestaurantsVMAsync(restaurants);
        }

        public async Task<IEnumerable<RestaurantBaseVM>> GetBaseAsync(IEnumerable<int> restaurantIds)
        {
            if (!restaurantIds.HasElement())
                return Enumerable.Empty<RestaurantBaseVM>();

            var restaurants = await _restaurantsRepository.GetByIdsAsync(restaurantIds);

            var restaurantIdsToImageGuids = await _imagesToRestaurantsRepository.GetRestaurantIdsToImageGuidsAsync(restaurants.Select(s => s.Id));

            return restaurants.Select(restaurant => new RestaurantBaseVM(
                restaurant.Id, 
                restaurant.Name, 
                restaurant.Address + ' ' + restaurant.House, //TODO сделать хелпер под это
                restaurantIdsToImageGuids.Where(w => w.Key == restaurant.Id).Select(s => RestaurantImageHelper.GetRestaurantImageUrl(restaurant.Id, _appSettings.FileService, s.Value)).FirstOrDefault()));
        }

        public async Task AddToFavoritesAsync(int userId, int restaurantId)
        {
            await _favoritesRestaurantsRepository.CreateAsync(new FavoritesRestaurantEntity 
            {
                UserId = userId,
                RestaurantId = restaurantId,
                IsActive = true
            });
        }

        public async Task RemoveFromFavoritesAsync(int userId, int restaurantId) =>
            await _favoritesRestaurantsRepository.RemoveAsync(userId, restaurantId);

        public async Task<IEnumerable<RestaurantVM>> GetFavoritesAsync(int take, int skip, int userId)
        {
            var favoritesRestaurantIds = await _favoritesRestaurantsRepository.GetActiveAsync(take, skip, userId);
            if (!favoritesRestaurantIds.HasElement())
                return Enumerable.Empty<RestaurantVM>();

            var restaurants = await _restaurantsRepository.GetByIdsAsync(favoritesRestaurantIds);

            return await ToRestaurantsVMAsync(restaurants);
        }

        private async Task<IEnumerable<RestaurantVM>> ToRestaurantsVMAsync(IEnumerable<RestaurantEntity> restaurants)
        {
            if (!restaurants.HasElement())
                return Enumerable.Empty<RestaurantVM>();

            var restaurantIdsToImageGuids = await _imagesToRestaurantsRepository.GetRestaurantIdsToImageGuidsAsync(restaurants.Select(s => s.Id));

            return restaurants.Select(restaurant =>
            {
                var restaurantVM = _mapper.Map<RestaurantVM>(restaurant);

                restaurantVM.ImageUrl = restaurantIdsToImageGuids
                    .Where(w => w.Key == restaurant.Id)
                    .Select(s => RestaurantImageHelper.GetRestaurantImageUrl(restaurant.Id, _appSettings.FileService, s.Value))
                    .FirstOrDefault();

                return restaurantVM;
            });
        }
    }
}
