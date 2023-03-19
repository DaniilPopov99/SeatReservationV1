namespace SeatReservationCore.Helpers.HttpHelper
{
    internal sealed class ApiExecutor<TResult> : SimpleApiExecutor where TResult : ApiBaseResult
    {
        private readonly IResponseParser<TResult> responseParser;

        public ApiExecutor(IApiResponseProducer apiResponseProducer, IResponseParser<TResult> responseParser, HttpClient httpClient)
            : base(apiResponseProducer, httpClient)
        {
            this.responseParser = responseParser;
        }
        public ApiExecutor(Func<HttpClient, Task<HttpResponseMessage>> apiResponseProducer, IResponseParser<TResult> responseParser, HttpClient httpClient)
            : base(apiResponseProducer, httpClient)
        {
            this.responseParser = responseParser;
        }

        public async Task<TResult> ExecuteAsync() => await TryGetResultFromResponseAsync(await GetResponseAsync());

        private async Task<TResult> TryGetResultFromResponseAsync(HttpResponseMessage response)
        {
            if (responseParser == null)
                return null;
            return await responseParser.TryParseAsync(response);
        }
    }

    internal class SimpleApiExecutor : IHttpClientUser
    {
        public HttpClient HttpClient { get; }
        private readonly Func<HttpClient, Task<HttpResponseMessage>> apiResponseProducer;

        public SimpleApiExecutor(IApiResponseProducer apiResponseProducer, HttpClient httpClient)
            : this((client) => apiResponseProducer.GetRespose(client), httpClient)
        {
        }
        public SimpleApiExecutor(Func<HttpClient, Task<HttpResponseMessage>> apiResponseProducer, HttpClient httpClient)
        {
            this.apiResponseProducer = apiResponseProducer;
            HttpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetResponseAsync() => await apiResponseProducer(HttpClient);
    }

    internal interface IHttpClientUser
    {
        HttpClient HttpClient { get; }
    }
}
