using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Models.Presentation
{
    public class UploadImageVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Content { get; set; }
    }
}
