using Newtonsoft.Json;
using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public sealed class HttpResultResponseParser : IResponseParser<HttpResult>
    {
        public async Task<HttpResult> TryParseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return HttpResultFactory.CreateEmptyResult(response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseBody))
                return HttpResultFactory.CreateEmptyResult(response.StatusCode);

            if (int.TryParse(responseBody, out int code))
                return HttpResultFactory.CreateErrors(response.StatusCode, code);
            try
            {
                var modelErrorCodes = JsonConvert.DeserializeObject<IEnumerable<int>>(responseBody);
                if (modelErrorCodes != null)
                    return HttpResultFactory.CreateErrors(response.StatusCode, modelErrorCodes);
            }
            catch
            {
                return HttpResultFactory.CreateErrors(response.StatusCode, -1);
            }
            return HttpResultFactory.CreateErrors(response.StatusCode, -1);
        }
    }

    public sealed class HttpResultResponseParser<TResult> : IResponseParser<HttpResult<TResult>>
    {
        public async Task<HttpResult<TResult>> TryParseAsync(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resultObject = JsonConvert.DeserializeObject<TResult>(responseBody);
                return HttpResultFactory.CreateResult(resultObject);
            }

            if (string.IsNullOrEmpty(responseBody))
            {
                return HttpResultFactory.CreateErrors<TResult>(response.StatusCode);
            }

            if (int.TryParse(responseBody, out int code))
            {
                return HttpResultFactory.CreateErrors<TResult>(response.StatusCode, code);
            }

            try
            {
                var modelErrorCodes = JsonConvert.DeserializeObject<IEnumerable<int>>(responseBody);
                if (modelErrorCodes != null)
                    return HttpResultFactory.CreateErrors<TResult>(response.StatusCode, modelErrorCodes);
            }
            catch
            {
                try
                {
                    return HttpResultFactory.CreateResult(JsonConvert.DeserializeObject<TResult>(responseBody), response.StatusCode);
                }
                catch
                {
                    return HttpResultFactory.CreateErrors<TResult>(response.StatusCode, -1);
                }
            }
            return HttpResultFactory.CreateErrors<TResult>(response.StatusCode, -1);
        }
    }
}
