using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IRestaurantManager
    {
        Task<int> CreateAsync(CreateRestaurantVM createModel);
        Task<IEnumerable<RestaurantVM>> GetByFilterAsync(RestaurantsFilterVM filter);
        Task<IEnumerable<RestaurantBaseVM>> GetBaseAsync(IEnumerable<int> restaurantIds);
    }
}
