namespace SeatReservationCore.Helpers
{
    public static class RestaurantImageHelper
    {
        public static string GetRestaurantImageUrl(int restaurantId, string domain, Guid? imageGuid) =>
            imageGuid.HasValue && imageGuid != Guid.Empty ? $"{domain}api/RestaurantImage/Get/{restaurantId}/{imageGuid.Value}" : null;

        public static string GetImageUrl(int imageId, string domain, Guid? imageGuid) =>
            imageGuid.HasValue && imageGuid != Guid.Empty ? $"{domain}api/RestaurantImage/GetImage/{imageId}/{imageGuid.Value}" : null;
    }
}
