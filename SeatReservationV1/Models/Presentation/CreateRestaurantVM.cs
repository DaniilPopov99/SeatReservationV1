using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Models.Presentation
{
    public class CreateRestaurantVM
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string House { get; set; }

        public IEnumerable<int> ImageIds { get; set; }
    }
}
