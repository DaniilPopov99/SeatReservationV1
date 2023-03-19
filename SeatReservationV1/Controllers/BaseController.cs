using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using SeatReservationV1.Extensions;
using SeatReservationV1.Helpers;
using System.Net;

namespace SeatReservationV1.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected IActionResult InternalServerError() => StatusCode((int)HttpStatusCode.InternalServerError);

        protected virtual async Task<IActionResult> Execute(Func<Task<IActionResult>> func)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                return await func();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        protected int GetUserId() => HttpContext.Request.GetUserIdFromHeader();

        protected string GetByKey(string key) => HttpContext.Request.GetByKeyFromHeader(key);

        protected MultipartReader GetMultipartReaderFromRequestBody()
        {
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 70);
            return new MultipartReader(boundary, Request.Body);
        }
    }
}
