using SeatReservationCore.Helpers;
using SeatReservationV1.Managers.Interfaces;
using SeatReservationV1.Models.Entities;
using SeatReservationV1.Models.Options;
using SeatReservationV1.Models.Presentation;
using SeatReservationV1.Repositories;
using System.Drawing;
using System.Drawing.Imaging;

namespace SeatReservationV1.Managers.Implementation
{
    public class RestaurantImageManager : IRestaurantImageManager
    {
        private readonly AppSettings _appSettings;

        private readonly ImagesRepository _imagesRepository;

        public RestaurantImageManager(AppSettings appSettings,
            ImagesRepository imagesRepository)
        {
            _appSettings = appSettings;
            _imagesRepository = imagesRepository;
        }

        public async Task<ImageVM> UploadAsync(UploadImageVM uploadModel)
        {
            var guid = Guid.NewGuid();

            var imageId = await _imagesRepository.CreateAsync(new ImageEntity 
            {
                Name = uploadModel.Name,
                Content = uploadModel.Content,
                CreateDate = DateTime.UtcNow,
                Guid = guid
            });

            //TODO сохраняем локально для обработки изображений ИИ(пока не реализовывал другую логику)
            var localName = $"{imageId}.jpg";
            var fileSavePath = Path.Combine(_appSettings.AllImageSaveDir, localName);

            MemoryStream memoryStream = new MemoryStream(uploadModel.Content);
            Image image = Image.FromStream(memoryStream);

            image.Save(fileSavePath, ImageFormat.Jpeg);

            return new ImageVM 
            {
                Id = imageId,
                Url = RestaurantImageHelper.GetImageUrl(imageId, _appSettings.FileService, guid)
            };
        }

        public async Task<byte[]> GetAsync(int restaurantId, Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            return await _imagesRepository.GetAsync(restaurantId, guid);
        }

        public async Task<byte[]> GetByImageIdAsync(int imageId, Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            return await _imagesRepository.GetByImageIdAsync(imageId, guid);
        }
    }
}
