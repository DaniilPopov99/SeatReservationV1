using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<int> RegisterAsync(RegisterUserVM userVM);
        Task<int> LoginAsync(string phoneNumber, string password);
        Task<UserVM> GetAsync(int userId);
        Task<IEnumerable<UserVM>> GetAsync(IEnumerable<int> userIds);
        Task<UserVMAndOrdersCount> GetUserAndOrdersCountAsync(int userId, int restaurantId);
    }
}
