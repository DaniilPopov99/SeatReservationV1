using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public class ApiBaseResult
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public ApiBaseResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public virtual IActionResult ToResult() => new StatusCodeResult((int)StatusCode);
    }
}
