using SeatReservationV1.Models.Presentation;

namespace SeatReservationV1.Managers.Interfaces
{
    public interface IRestaurantImageManager
    {
        Task<int> UploadAsync(UploadImageVM uploadModel);
        Task<byte[]> GetAsync(int restaurantId, Guid guid);
    }
}
