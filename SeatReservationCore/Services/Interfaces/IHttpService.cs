using SeatReservationCore.Helpers.HttpHelper;

namespace SeatReservationCore.Services.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResult> ExecuteGetAsync(string getUrl);
        Task<HttpResult<TResult>> ExecuteGetAsync<TResult>(string getUrl);
        Task<HttpResult> ExecutePostAsync(string postUrl, HttpContent content);
        Task<HttpResult> ExecutePostAsync(string postUrl, object jsonObject);
        Task<HttpResult<TResult>> ExecutePostAsync<TResult>(string postUrl, object jsonObject);
    }
}
