namespace SeatReservationCore.Helpers.HttpHelper
{
    internal sealed class ApiExecutorBuilder
    {
        internal ApiExecutorResponseParserAdder AddResponseProducer(Func<HttpClient, Task<HttpResponseMessage>> apiResponseProducer, HttpClient httpClient) => new ApiExecutorResponseParserAdder(apiResponseProducer, httpClient);
        internal ApiExecutorResponseParserAdder AddResponseProducer(IApiResponseProducer apiResponseProducer, HttpClient httpClient) => new ApiExecutorResponseParserAdder((client) => apiResponseProducer.GetRespose(client), httpClient);

        internal class ApiExecutorResponseParserAdder
        {
            private readonly HttpClient _httpClient;
            private readonly Func<HttpClient, Task<HttpResponseMessage>> _apiResponseProducer;

            internal ApiExecutorResponseParserAdder(Func<HttpClient, Task<HttpResponseMessage>> apiResponseProducer, HttpClient httpClient)
            {
                _apiResponseProducer = apiResponseProducer;
                _httpClient = httpClient;
            }

            public ApiExecutor<TResult> AddResponseParser<TResult>(IResponseParser<TResult> responseParser) where TResult : ApiBaseResult =>
                new ApiExecutor<TResult>(_apiResponseProducer, responseParser, _httpClient);

            public SimpleApiExecutor ToSimpleApiExecutor() => new SimpleApiExecutor(_apiResponseProducer, _httpClient);
        }
    }
}
