using Newtonsoft.Json;
using SeatReservationCore.Helpers.HttpHelper;
using SeatReservationCore.Services.Interfaces;
using System.Text;

namespace SeatReservationCore.Services.Implementation
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResult> ExecuteGetAsync(string getUrl)
        {
            return await new ApiExecutorBuilder()
                .AddResponseProducer((httpClient) => httpClient.GetAsync(new Uri(getUrl)), _httpClient)
                .AddResponseParser(new HttpResultResponseParser())
                .ExecuteAsync();
        }

        public async Task<HttpResult<TResult>> ExecuteGetAsync<TResult>(string getUrl)
        {
            return await new ApiExecutorBuilder()
                .AddResponseProducer((httpClient) => httpClient.GetAsync(new Uri(getUrl)), _httpClient)
                .AddResponseParser(new HttpResultResponseParser<TResult>())
                .ExecuteAsync();
        }

        public async Task<HttpResult> ExecutePostAsync(string postUrl, HttpContent content)
        {
            return await new ApiExecutorBuilder()
                .AddResponseProducer((httpClient) => httpClient.PostAsync(new Uri(postUrl), content), _httpClient)
                .AddResponseParser(new HttpResultResponseParser())
                .ExecuteAsync();
        }

        public async Task<HttpResult> ExecutePostAsync(string postUrl, object jsonObject) =>
            await ExecutePostAsync(postUrl, GetStringContent(jsonObject));

        public async Task<HttpResult<TResult>> ExecutePostAsync<TResult>(string postUrl, object jsonObject) =>
            await ExecutePostAsync<TResult>(postUrl, GetStringContent(jsonObject));

        private StringContent GetStringContent(Object jsonObject)
        {
            if (jsonObject == null)
                return null;

            return new StringContent(JsonConvert.SerializeObject(jsonObject), Encoding.UTF8, "application/json");
        }
    }
}
