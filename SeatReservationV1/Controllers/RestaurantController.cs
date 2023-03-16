using Microsoft.AspNetCore.Mvc;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Presentation;
using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantManager _restaurantManager;

        public RestaurantController(IRestaurantManager restaurantManager)
        {
            _restaurantManager = restaurantManager;
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create([FromBody, Required] CreateRestaurantVM createModel)
        {
            return await Execute(async () =>
            {
                return Ok(await _restaurantManager.CreateAsync(createModel));
            });
        }

        [HttpPost(nameof(GetByFilter))]
        public async Task<IActionResult> GetByFilter([FromBody, Required] RestaurantsFilterVM filter)
        {
            return await Execute(async () =>
            {
                return Ok(await _restaurantManager.GetByFilterAsync(filter));
            });
        }
    }
}
