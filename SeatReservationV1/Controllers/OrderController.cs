using Microsoft.AspNetCore.Mvc;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Presentation;
using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        #region Get

        [HttpGet(nameof(GetActive))]
        public async Task<IActionResult> GetActive([FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                return Ok(await _orderManager.GetActiveAsync(GetUserId()));
            });
        }

        [HttpGet(nameof(GetHistory) + "/{take}/{skip}")]
        public async Task<IActionResult> GetHistory([FromRoute, Range(1, int.MaxValue)] int take, [FromRoute, Range(0, int.MaxValue)] int skip, [FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                return Ok(await _orderManager.GetHistoryAsync(take, skip, GetUserId()));
            });
        }

        [HttpGet(nameof(GetActiveByRestaurantId) + "/{take}/{skip}/{restaurantId}")]
        public async Task<IActionResult> GetActiveByRestaurantId([FromRoute, Range(1, int.MaxValue)] int take, [FromRoute, Range(0, int.MaxValue)] int skip, [FromRoute, Range(0, int.MaxValue)] int restaurantId)
        {
            return await Execute(async () =>
            {
                return Ok(await _orderManager.GetActiveByRestaurantIdAsync(take, skip, restaurantId));
            });
        }

        [HttpGet(nameof(GetInactiveByRestaurantId) + "/{take}/{skip}/{restaurantId}")]
        public async Task<IActionResult> GetInactiveByRestaurantId([FromRoute, Range(1, int.MaxValue)] int take, [FromRoute, Range(0, int.MaxValue)] int skip, [FromRoute, Range(0, int.MaxValue)] int restaurantId)
        {
            return await Execute(async () =>
            {
                return Ok(await _orderManager.GetInactiveByRestaurantIdAsync(take, skip, restaurantId));
            });
        }

        #endregion

        #region Post

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create([FromBody, Required] CreateOrderVM createModel, [FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                return Ok(await _orderManager.CreateAsync(createModel, GetUserId()));
            });
        }

        #endregion
    }
}
