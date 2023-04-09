using Microsoft.AspNetCore.Mvc;
using SeatReservationV1.Managers.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        #region Get

        [HttpGet(nameof(GetProfile) + "/{restaurantId}/{userId}")]
        public async Task<IActionResult> GetProfile([FromRoute, Range(1, int.MaxValue)] int restaurantId, [FromRoute, Range(1, int.MaxValue)] int userId)
        {
            return await Execute(async () =>
            {
                return Ok(await _userManager.GetUserAndOrdersCountAsync(userId, restaurantId));
            });
        }

        #endregion
    }
}
