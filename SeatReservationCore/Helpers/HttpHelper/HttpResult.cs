using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public sealed class HttpResult : ApiResultWithObject<IEnumerable<Int32>>
    {
        public Boolean ExistErrorContent { get; }
        public Boolean IsOK => StatusCode == HttpStatusCode.OK;

        private HttpResult(HttpStatusCode statusCode, IEnumerable<Int32> resultModel) : base(statusCode, resultModel)
        {
            ExistErrorContent = resultModel != null;
        }

        public static HttpResult CreateWithEmptyContent(HttpStatusCode statusCode) => new HttpResult(statusCode, null);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, IEnumerable<Int32> errors) => new HttpResult(statusCode, errors);
        public static HttpResult CreateErrors(HttpStatusCode statusCode, params Int32[] errors) => CreateErrors(statusCode, errors as IEnumerable<Int32>);

        public override IActionResult ToResult()
        {
            if (!ExistErrorContent)
                return new StatusCodeResult((Int32)StatusCode);
            return base.ToResult();
        }
    }

    public sealed class HttpResult<TResult> : ApiTwoFacedResult<TResult, IEnumerable<Int32>>
    {
        private HttpResult(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) : base(resultModel, statusCode) { }
        private HttpResult(HttpStatusCode statusCodeForError, IEnumerable<int> errorModel) : base(statusCodeForError, errorModel) { }

        public static HttpResult<TResult> CreateResult(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK) => new HttpResult<TResult>(resultModel, statusCode);
        public static HttpResult<TResult> CreateErrors(HttpStatusCode statusCodeForError, IEnumerable<Int32> errors) => new HttpResult<TResult>(statusCodeForError, errors);
        public static HttpResult<TResult> CreateErrors(HttpStatusCode statusCodeForError, params Int32[] errors) => CreateErrors(statusCodeForError, errors as IEnumerable<Int32>);
    }
}
