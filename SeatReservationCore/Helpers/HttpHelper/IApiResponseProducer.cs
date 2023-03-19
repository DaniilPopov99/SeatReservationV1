namespace SeatReservationCore.Helpers.HttpHelper
{
    internal interface IApiResponseProducer
    {
        Task<HttpResponseMessage> GetRespose(HttpClient httpClient);
    }
}
