using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public static class HttpResultFactory
    {
        public static HttpResult<TResult> CreateResult<TResult>(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            HttpResult<TResult>.CreateResult(resultModel, statusCode);
        public static HttpResult<TResult> CreateErrors<TResult>(HttpStatusCode statusCodeForError, IEnumerable<int> errors) =>
            HttpResult<TResult>.CreateErrors(statusCodeForError, errors);
        public static HttpResult<TResult> CreateErrors<TResult>(HttpStatusCode statusCodeForError, params int[] errors) =>
            HttpResult<TResult>.CreateErrors(statusCodeForError, errors);

        public static HttpResult CreateEmptyResult(HttpStatusCode statusCode = HttpStatusCode.OK) => HttpResult.CreateWithEmptyContent(statusCode);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, IEnumerable<int> errors) => HttpResult.CreateErrors(statusCode, errors);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, params int[] errors) => HttpResult.CreateErrors(statusCode, errors);
    }
}
