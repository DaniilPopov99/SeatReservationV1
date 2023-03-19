using SeatReservationCore.Helpers.HttpHelper;
using SeatReservationCore.Services.Interfaces;
using SeatReservationV1.Microservices.Interfaces;
using SeatReservationV1.Models.Options;

namespace SeatReservationV1.Microservices.Implementation
{
    public class SimilarImagesMicroservice : ISimilarImagesMicroservice
    {
        private string _domain;
        private readonly IHttpService _httpService;

        public SimilarImagesMicroservice(
            AppSettings settings,
            IHttpService httpService)
        {
            _domain = settings.SimilarImagesService;
            _httpService = httpService;
        }

        public async Task IndexingAsync()
            => await _httpService.ExecuteGetAsync($"{_domain}api/Indexing");

        public async Task<IEnumerable<int>> SearchAsync(int imageId)
            => HttpResultHelper<IEnumerable<int>>.GetResultOrThrow(
                await _httpService.ExecuteGetAsync<IEnumerable<int>>($"{_domain}api/Search/{imageId}"));
    }
}
