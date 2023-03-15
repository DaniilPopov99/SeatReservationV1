using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Models.Presentation
{
    public class RegisterUserVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
