using SeatReservationCore.Extensions;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;

namespace SeatReservationV1.Managers.Implementation
{
    public class RestaurantManager : IRestaurantManager
    {
        private readonly RestaurantsRepository _restaurantsRepository;
        private readonly ImagesToRestaurantsRepository _imagesToRestaurantsRepository;

        public RestaurantManager(RestaurantsRepository restaurantsRepository,
            ImagesToRestaurantsRepository imagesToRestaurantsRepository)
        {
            _restaurantsRepository = restaurantsRepository;
            _imagesToRestaurantsRepository = imagesToRestaurantsRepository;
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
                CreateDate = DateTime.UtcNow
            });
            
            if (createModel.ImageIds.HasElement())
            {
                await _imagesToRestaurantsRepository.CreateAsync(createModel.ImageIds.Select(imageId => new ImageToRestaurantEntity 
                { 
                    RestaurantId = restaurantId,
                    ImageId = imageId
                }));
            }

            return restaurantId;
        }

        public async Task<IEnumerable<RestaurantVM>> GetByFilterAsync(RestaurantsFilterVM filter)
        {
            if (filter.ImageId > 0)
            {
                //TODO нужно получить фотки из бд которые больше всего похожи на эту фотографию
            }

            var restaurants = await _restaurantsRepository.GetByFilterAsync(filter.Take, filter.Skip, filter.Filter);

            return restaurants.Select(restaurant => new RestaurantVM 
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description=restaurant.Description,
                FullAddress = restaurant.Address + ' ' + restaurant.House,
                PhoneNumber = restaurant.PhoneNumber,
                ImageUrl = string.Empty
            });
        }
    }
}
