namespace SeatReservationV1.Microservices.Interfaces
{
    public interface ISimilarImagesMicroservice
    {
        Task IndexingAsync();
        Task<IEnumerable<int>> SearchAsync(int imageId);
    }
}
