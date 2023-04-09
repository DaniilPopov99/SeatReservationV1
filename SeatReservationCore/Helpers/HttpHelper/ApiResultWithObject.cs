using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public class ApiResultWithObject<TResult> : ApiBaseResult
    {
        public TResult ResultModel { get; protected set; }

        public ApiResultWithObject(HttpStatusCode statusCode, TResult resultModel) : base(statusCode)
        {
            ResultModel = resultModel;
        }

        public override IActionResult ToResult()
        {
            var objectResult = new ObjectResult(ResultModel);
            objectResult.StatusCode = (int)StatusCode;

            return objectResult;
        }
    }
}
