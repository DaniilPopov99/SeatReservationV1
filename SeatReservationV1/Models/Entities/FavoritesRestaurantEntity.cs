using Dapper.Contrib.Extensions;

namespace SeatReservationV1.Models.Entities
{
    [Table("dbo.FavoritesRestaurants")]
    public class FavoritesRestaurantEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public bool IsActive { get; set; }
    }
}
