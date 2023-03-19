using SeatReservationCore.Extensions;
using SeatReservationCore.Helpers;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Microservices.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Options;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace SeatReservationV1.Managers.Implementation
{
    public class RestaurantManager : IRestaurantManager
    {
        private readonly AppSettings _appSettings;

        private readonly ISimilarImagesMicroservice _similarImagesMicroservice;

        private readonly RestaurantsRepository _restaurantsRepository;
        private readonly ImagesToRestaurantsRepository _imagesToRestaurantsRepository;
        private readonly FavoritesRestaurantsRepository _favoritesRestaurantsRepository;

        public RestaurantManager(AppSettings appSettings,
            ISimilarImagesMicroservice similarImagesMicroservice,
            RestaurantsRepository restaurantsRepository,
            ImagesToRestaurantsRepository imagesToRestaurantsRepository,
            FavoritesRestaurantsRepository favoritesRestaurantsRepository)
        {
            _appSettings = appSettings;
            _similarImagesMicroservice = similarImagesMicroservice;
            _restaurantsRepository = restaurantsRepository;
            _imagesToRestaurantsRepository = imagesToRestaurantsRepository;
            _favoritesRestaurantsRepository = favoritesRestaurantsRepository;
        }

        public async Task<int> CreateAsync(CreateRestaurantVM createModel)
        {
            var restaurantId = await _restaurantsRepository.CreateAsync(new RestaurantEntity
            {
                Name = createModel.Name,
                Description = createModel.Description,
                PhoneNumber = createModel.PhoneNumber,
                Address = createModel.Address,
                House = createModel.House,
                CreateDate = DateTime.UtcNow,
                IsActive = true
            });
            
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

            if (!restaurants.HasElement())
                return Enumerable.Empty<RestaurantVM>();

            var restaurantIdsToImageGuids = await _imagesToRestaurantsRepository.GetRestaurantIdsToImageGuidsAsync(restaurants.Select(s => s.Id));

            return restaurants.Select(restaurant => new RestaurantVM 
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description=restaurant.Description,
                FullAddress = restaurant.Address + ' ' + restaurant.House,
                PhoneNumber = restaurant.PhoneNumber,
                ImageUrl = restaurantIdsToImageGuids.Where(w => w.Key == restaurant.Id).Select(s => RestaurantImageHelper.GetRestaurantImageUrl(restaurant.Id, _appSettings.FileService, s.Value)).FirstOrDefault()
            });
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
                restaurant.Address + ' ' + restaurant.House,
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

            var restaurantIdsToImageGuids = await _imagesToRestaurantsRepository.GetRestaurantIdsToImageGuidsAsync(restaurants.Select(s => s.Id));

            return restaurants.Select(restaurant => new RestaurantVM
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                FullAddress = restaurant.Address + ' ' + restaurant.House,
                PhoneNumber = restaurant.PhoneNumber,
                ImageUrl = restaurantIdsToImageGuids.Where(w => w.Key == restaurant.Id).Select(s => RestaurantImageHelper.GetRestaurantImageUrl(restaurant.Id, _appSettings.FileService, s.Value)).FirstOrDefault()
            });
        }
    }
}
