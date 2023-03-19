namespace SeatReservationV1.Microservices.Interfaces
{
    public interface ISimilarImagesMicroservice
    {
        Task PostAsync(int model);
        Task<IEnumerable<int>> GetIdsAsync(int model);
    }
}
