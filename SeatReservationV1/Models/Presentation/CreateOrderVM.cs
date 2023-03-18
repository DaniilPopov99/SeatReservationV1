using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Models.Presentation
{
    public class CreateOrderVM
    {
        [Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }

        public DateTime Date { get; set; }

        [Range(1, int.MaxValue)]
        public int PersonCount { get; set; }

        public string Comment { get; set; }
    }
}
