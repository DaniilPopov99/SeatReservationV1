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
                    {
                        if (response.ErrorModel?.FirstOrDefault() != null)
                        {
                            throw new Exception();
                        }
                        throw new Exception();
                    }
                case HttpStatusCode.Forbidden:
                    {
                        if (response.ErrorModel?.FirstOrDefault() != null)
                        {
                            throw new Exception();//(response.ErrorModel.FirstOrDefault());
                        }

                        throw new Exception();
                    }
                case HttpStatusCode.Conflict:
                    {
                        if (response.ErrorModel?.FirstOrDefault() != null)
                            throw new Exception();

                        throw new Exception();//(response.ResultModel);
                    }
                case HttpStatusCode.BadRequest:
                    {
                        if (response.ErrorModel?.FirstOrDefault() != null)
                            throw new Exception();
                        else
                            throw new Exception();
                    }
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
                    if (int.TryParse(response.ResultModel?.FirstOrDefault().ToString(), out int notFoundErrorCode))
                        return new Exception();
                    else
                        return new Exception();
                case HttpStatusCode.Forbidden:
                    if (int.TryParse(response.ResultModel?.FirstOrDefault().ToString(), out int forbidErrorCode))
                        return new Exception();//(forbidErrorCode);
                    else
                        return new Exception();
                case HttpStatusCode.Conflict:
                    return int.TryParse(response.ResultModel?.FirstOrDefault().ToString(), out int conflictErrorCode)
                        ? new Exception()
                        : new Exception();
                case HttpStatusCode.BadRequest:
                    if (int.TryParse(response.ResultModel?.FirstOrDefault().ToString(), out int badRequestErrorCode))
                        return new Exception();
                    else
                        return new Exception();
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
