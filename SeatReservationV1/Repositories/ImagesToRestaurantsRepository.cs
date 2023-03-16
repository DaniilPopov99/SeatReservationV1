using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class ImagesToRestaurantsRepository : GenericRepository<ImageToRestaurantEntity>
    {
        public ImagesToRestaurantsRepository(string connectionString) : base(connectionString) { }
    }
}
