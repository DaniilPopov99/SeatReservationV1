using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<int> RegisterAsync(RegisterUserVM userVM);
        Task<int> LoginAsync(string phoneNumber, string password);
    }
}
