using Dapper.Contrib.Extensions;

namespace SeatReservationV1.Models.Entities
{
    [Table("dbo.Images")]
    public class ImageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Guid { get; set; }
    }
}
