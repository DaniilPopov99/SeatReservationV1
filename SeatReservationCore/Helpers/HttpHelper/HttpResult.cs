using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public sealed class HttpResult : ApiResultWithObject<IEnumerable<int>>
    {
        public bool ExistErrorContent { get; }
        public bool IsOK => StatusCode == HttpStatusCode.OK;

        private HttpResult(HttpStatusCode statusCode, IEnumerable<int> resultModel) : base(statusCode, resultModel)
        {
            ExistErrorContent = resultModel != null;
        }

        public static HttpResult CreateWithEmptyContent(HttpStatusCode statusCode) => new HttpResult(statusCode, null);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, IEnumerable<int> errors) => new HttpResult(statusCode, errors);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, params int[] errors) => CreateErrors(statusCode, errors as IEnumerable<int>);

        public override IActionResult ToResult()
        {
            if (!ExistErrorContent)
                return new StatusCodeResult((int)StatusCode);

            return base.ToResult();
        }
    }

    public sealed class HttpResult<TResult> : ApiTwoFacedResult<TResult, IEnumerable<int>>
    {
        private HttpResult(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) : base(resultModel, statusCode) { }
        private HttpResult(HttpStatusCode statusCodeForError, IEnumerable<int> errorModel) : base(statusCodeForError, errorModel) { }

        public static HttpResult<TResult> CreateResult(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) => new HttpResult<TResult>(resultModel, statusCode);
        public static HttpResult<TResult> CreateErrors(HttpStatusCode statusCodeForError, IEnumerable<int> errors) => new HttpResult<TResult>(statusCodeForError, errors);
        public static HttpResult<TResult> CreateErrors(HttpStatusCode statusCodeForError, params int[] errors) => CreateErrors(statusCodeForError, errors as IEnumerable<Int32>);
    }
}
