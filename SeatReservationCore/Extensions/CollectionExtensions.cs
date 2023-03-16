namespace SeatReservationCore.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasElement<T>(this IEnumerable<T> source) => source?.Any() ?? false;
    }
}
