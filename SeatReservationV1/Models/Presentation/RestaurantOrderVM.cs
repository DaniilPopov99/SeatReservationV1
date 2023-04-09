namespace SeatReservationV1.Models.Presentation
{
    public class RestaurantOrderVM
    {
        public int OrderId { get; set; }
        public UserVM User { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreateDate { get; set; }
        public int PersonCount { get; set; }
        public string Comment { get; set; }
    }
}
