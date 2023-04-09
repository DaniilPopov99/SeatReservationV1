using System.Net;

namespace SeatReservationCore.Helpers.HttpHelper
{
    public static class HttpResultHelper<TResult>
    {
        public static TResult GetResultOrThrow(HttpResult<TResult> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    return response.ResultModel;

                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                    return response.ErrorModel?.FirstOrDefault() != null
                        ? throw new Exception()
                        : throw new Exception();

                case HttpStatusCode.InternalServerError:
                    throw new Exception(nameof(response));

                case HttpStatusCode.Unauthorized:
                    throw new Exception();

                default:
                    throw new ArgumentException(nameof(response));
            }
        }
    }

    public static class HttpResultHelper
    {
        public static Exception GetException(HttpResult response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Accepted:
                    return null;

                case HttpStatusCode.NotFound:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                    return int.TryParse(response.ResultModel?.FirstOrDefault().ToString(), out int errorCode)
                        ? new Exception()
                        : new Exception();

                case HttpStatusCode.InternalServerError:
                    throw new Exception();

                case HttpStatusCode.Unauthorized:
                    throw new Exception();

                default:
                    return new ArgumentException(nameof(response));
            }
        }

        public static void DoOrThrowException(HttpResult response)
        {
            var ex = GetException(response);

            if (ex != null)
                throw ex;
        }
    }
}
