namespace SeatReservationV1.Models.Presentation
{
    public class RestaurantBaseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }

        public RestaurantBaseVM(int id, string name, string address, string imageUrl)
        {
            Id = id;
            Name = name;
            Address = address;
            ImageUrl = imageUrl;
        }
    }
}
