using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Models.Presentation
{
    public class RestaurantsFilterVM
    {
        [Range(1, int.MaxValue)]
        public int Take { get; set; }

        public int Skip { get; set; }

        public string Filter { get; set; }

        public int? ImageId { get; set; }
    }
}
