using Dapper.Contrib.Extensions;

namespace SeatReservationV1.Models.Entities
{
    [Table("dbo.Orders")]
    public class OrderEntity
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreateDate { get; set; }
        public int PersonCount { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
    }
}
