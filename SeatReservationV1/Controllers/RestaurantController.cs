﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost(nameof(AddToFavorites))]
        public async Task<IActionResult> AddToFavorites([Range(1, int.MaxValue)] int restaurantId, [FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                await _restaurantManager.AddToFavoritesAsync(GetUserId(), restaurantId);
                return Ok();
            });
        }

        [HttpPost(nameof(RemoveFromFavorites))]
        public async Task<IActionResult> RemoveFromFavorites([Range(1, int.MaxValue)] int restaurantId, [FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                await _restaurantManager.RemoveFromFavoritesAsync(GetUserId(), restaurantId);
                return Ok();
            });
        }

        [HttpGet(nameof(GetFavorites) + "/{take}/{skip}")]
        public async Task<IActionResult> GetFavorites([FromRoute, Range(1, int.MaxValue)] int take, [FromRoute, Range(0, int.MaxValue)] int skip, [FromHeader] int userId)
        {
            return await Execute(async () =>
            {
                return Ok(await _restaurantManager.GetFavoritesAsync(take, skip, GetUserId()));
            });
        }
    }
}
