using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IOrderManager
    {
        Task<int> CreateAsync(CreateOrderVM createModel, int userId);
        Task<IEnumerable<UserOrderVM>> GetActiveAsync(int userId);
        Task<IEnumerable<UserOrderVM>> GetHistoryAsync(int take, int skip, int userId);
        Task<IEnumerable<RestaurantOrderVM>> GetActiveByRestaurantIdAsync(int take, int skip, int restaurantId);
        Task<IEnumerable<RestaurantOrderVM>> GetInactiveByRestaurantIdAsync(int take, int skip, int restaurantId);
    }
}
