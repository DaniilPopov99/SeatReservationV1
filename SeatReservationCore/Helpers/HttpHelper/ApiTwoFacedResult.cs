using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public class ApiTwoFacedResult<TResult, TError> : ApiBaseResult
    {
        public Boolean ExistError { get; protected set; } = false;
        public TResult ResultModel { get; protected set; }
        public TError ErrorModel { get; protected set; }

        public ApiTwoFacedResult(TResult resultModel, HttpStatusCode statusCode = HttpStatusCode.OK)
            : base(statusCode)
        {
            ResultModel = resultModel;
        }

        public ApiTwoFacedResult(HttpStatusCode statusCodeForError, TError errorModel)
            : base(statusCodeForError)
        {
            ErrorModel = errorModel;
            ExistError = true;
        }

        public override IActionResult ToResult()
        {
            if (ExistError)
                return ToErrorResult();
            return ToObjectResult();
        }

        public IActionResult ToErrorResult() => ToObjectResult(ErrorModel);
        public IActionResult ToObjectResult() => ToObjectResult(ResultModel);

        private ObjectResult ToObjectResult<T>(T value)
        {
            var objectResult = new ObjectResult(value);
            objectResult.StatusCode = (Int32)StatusCode;

            //fix "Write to non-body 204 response"
            if (StatusCode == HttpStatusCode.NoContent)
                return new ObjectResult(null);

            return objectResult;
        }
    }
}
