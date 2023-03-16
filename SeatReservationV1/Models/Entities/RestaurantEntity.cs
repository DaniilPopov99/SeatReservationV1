using Dapper.Contrib.Extensions;

namespace SeatReservationV1.Models.Entities
{
    [Table("dbo.Restaurants")]
    public class RestaurantEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string House { get; set; }
        public DateTime CreateDate { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
