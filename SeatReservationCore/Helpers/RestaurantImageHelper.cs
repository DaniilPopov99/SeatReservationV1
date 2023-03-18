namespace SeatReservationCore.Helpers
{
    public static class RestaurantImageHelper
    {
        public static string GetRestaurantImageUrl(int restaurantId, string domain, Guid? imageGuid) =>
            imageGuid.HasValue && imageGuid != Guid.Empty ? $"{domain}api/RestaurantImage/Get/{restaurantId}/{imageGuid.Value}" : null;
    }
}
