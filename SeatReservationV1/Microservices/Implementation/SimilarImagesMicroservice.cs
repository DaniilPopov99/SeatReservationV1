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
            _domain = settings.FileService;
            _httpService = httpService;
        }

        public async Task PostAsync(int model)
            => HttpResultHelper.DoOrThrowException(
                await _httpService.ExecutePostAsync($"{_domain}api/Post", model));

        public async Task<IEnumerable<int>> GetIdsAsync(int model)
            => HttpResultHelper<IEnumerable<int>>.GetResultOrThrow(
                await _httpService.ExecutePostAsync<IEnumerable<int>>($"{_domain}api/Get", model));
    }
}
