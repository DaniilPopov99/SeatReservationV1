using Microsoft.AspNetCore.Mvc;
using SeatReservationCore.Extensions;
using SeatReservationV1.Helpers;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Presentation;
using System.ComponentModel.DataAnnotations;

namespace SeatReservationV1.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantImageController : BaseController
    {
        private readonly IRestaurantImageManager _restaurantImageManager;

        public RestaurantImageController(IRestaurantImageManager restaurantImageManager)
        {
            _restaurantImageManager = restaurantImageManager;
        }

        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload()
        {
            return await Execute(async () =>
            {
                if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                {
                    throw new Exception($"Expected a multipart request, but got {Request.ContentType}");
                }

                var reader = GetMultipartReaderFromRequestBody();
                var section = await reader.ReadNextSectionAsync();
                var fileContent = await MultipartRequestHelper.GetMultipartSectionContentBytes(section);

                return Ok(await _restaurantImageManager.UploadAsync(new UploadImageVM 
                {
                    Name = MultipartRequestHelper.TryGetMultipartSectionHeaderValue<string>(section, "FileName"),
                    Content = fileContent
                }));
            });
        }

        [HttpGet(nameof(Get) + "/{restaurantId}/{guid}")]
        public async Task<IActionResult> Get([FromRoute, Range(1, int.MaxValue)] int restaurantId, [FromRoute] Guid guid)
        {
            return await Execute(async () =>
            {
                var result = await _restaurantImageManager.GetAsync(restaurantId, guid);
                if (!result.HasElement())
                {
                    throw new Exception();
                }

                return File(result, "image/jpeg");
            });
        }
    }
}
