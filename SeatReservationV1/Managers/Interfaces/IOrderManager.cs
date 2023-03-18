using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IOrderManager
    {
        Task<int> CreateAsync(CreateOrderVM createModel, int userId);
        Task<IEnumerable<OrderVM>> GetActiveAsync(int userId);
        Task<IEnumerable<OrderVM>> GetHistoryAsync(int take, int skip, int userId);
    }
}
