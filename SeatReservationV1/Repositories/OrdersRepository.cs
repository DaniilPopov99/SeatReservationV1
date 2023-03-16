using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class OrdersRepository : GenericRepository<OrderEntity>
    {
        public OrdersRepository(string connectionString) : base(connectionString) { }
    }
}
