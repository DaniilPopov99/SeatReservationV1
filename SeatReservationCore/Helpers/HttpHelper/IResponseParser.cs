namespace SeatReservationCore.Helpers.HttpHelper
{
    internal interface IResponseParser<TResult> where TResult : ApiBaseResult
    {
        Task<TResult> TryParseAsync(HttpResponseMessage response);
    }
}
