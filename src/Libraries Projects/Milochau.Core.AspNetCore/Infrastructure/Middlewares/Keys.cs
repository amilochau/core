namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    internal static class Keys
    {
        public const string JsonResponseType = "application/json";
        public const string TextResponseType = "text/plain";

        public const string GetMethod = "GET";
        public const string PostMethod = "POST";

        public const string EndpointRouteNotFoundMessage = "Endpoint route has not been found.";
    }
}
