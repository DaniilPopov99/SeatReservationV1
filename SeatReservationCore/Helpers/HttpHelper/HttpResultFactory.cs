using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public static class HttpResultFactory
    {
        public static HttpResult<TResult> CreateResult<TResult>(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            HttpResult<TResult>.CreateResult(resultModel, statusCode);
        public static HttpResult<TResult> CreateErrors<TResult>(HttpStatusCode statusCodeForError, IEnumerable<Int32> errors) =>
            HttpResult<TResult>.CreateErrors(statusCodeForError, errors);
        public static HttpResult<TResult> CreateErrors<TResult>(HttpStatusCode statusCodeForError, params Int32[] errors) =>
            HttpResult<TResult>.CreateErrors(statusCodeForError, errors);

        public static HttpResult CreateEmptyResult(HttpStatusCode statusCode = HttpStatusCode.OK) => HttpResult.CreateWithEmptyContent(statusCode);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, IEnumerable<Int32> errors) => HttpResult.CreateErrors(statusCode, errors);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, params Int32[] errors) => HttpResult.CreateErrors(statusCode, errors);
    }
}
