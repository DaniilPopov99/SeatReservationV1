using Microsoft.AspNetCore.Mvc;
using SeatReservationV1.Extensions;
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
    }
}
