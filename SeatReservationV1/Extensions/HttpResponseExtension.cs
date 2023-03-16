namespace SeatReservationV1.Extensions
{
    public static class HttpResponseExtension
    {
        public static int GetUserIdFromHeader(this HttpRequest httpRequest) =>
            int.TryParse(httpRequest.Headers["userId"], out int userId) ? userId : throw new Exception();
    }
}
