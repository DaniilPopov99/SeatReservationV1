using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IRestaurantManager
    {
        Task<int> CreateAsync(CreateRestaurantVM createModel);
        Task<IEnumerable<RestaurantVM>> GetByFilterAsync(RestaurantsFilterVM filter);
        Task<IEnumerable<RestaurantBaseVM>> GetBaseAsync(IEnumerable<int> restaurantIds);
        Task AddToFavoritesAsync(int userId, int restaurantId);
        Task RemoveFromFavoritesAsync(int userId, int restaurantId);
        Task<IEnumerable<RestaurantVM>> GetFavoritesAsync(int take, int skip, int userId);
    }
}
