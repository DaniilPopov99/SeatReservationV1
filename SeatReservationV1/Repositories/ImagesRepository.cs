using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class ImagesRepository : GenericRepository<ImageEntity>
    {
        public ImagesRepository(string connectionString) : base(connectionString) { }
    }
}
