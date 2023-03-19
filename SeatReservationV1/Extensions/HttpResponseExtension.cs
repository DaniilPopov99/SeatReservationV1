namespace SeatReservationV1.Extensions
{
    public static class HttpResponseExtension
    {
        public static int GetUserIdFromHeader(this HttpRequest httpRequest) =>
            int.TryParse(httpRequest.Headers["userId"], out int userId) ? userId : throw new Exception();

        public static string GetByKeyFromHeader(this HttpRequest httpRequest, string key) =>
            !string.IsNullOrEmpty(httpRequest.Headers[key]) ? httpRequest.Headers[key] : throw new Exception();
    }
}
