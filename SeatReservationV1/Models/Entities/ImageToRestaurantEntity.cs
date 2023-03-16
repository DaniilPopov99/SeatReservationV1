using Dapper.Contrib.Extensions;

namespace SeatReservationV1.Models.Entities
{
    [Table("dbo.ImagesToRestaurants")]
    public class ImageToRestaurantEntity
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int ImageId { get; set; }
        public bool IsActive { get; set; }
    }
}
