using SeatReservationCore.Repositories;
using SeatReservationV1.Models.Entities;

namespace SeatReservationV1.Repositories
{
    public class FavoritesRestaurantsRepository : GenericRepository<FavoritesRestaurantEntity>
    {
        public FavoritesRestaurantsRepository(string connectionString) : base(connectionString) { }
    }
}
