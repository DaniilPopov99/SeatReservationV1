﻿using Microsoft.AspNetCore.Mvc;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Presentation;
using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IUserManager _userManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody, Required] RegisterUserVM userVM)
        {
            return await Execute(async () =>
            {
                return Ok(await _userManager.RegisterAsync(userVM));
            });
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody, Required] LoginVM login)
        {
            return await Execute(async () =>
            {
                return Ok(await _userManager.LoginAsync(login.PhoneNumber, login.Password));
            });
        }
    }
}
