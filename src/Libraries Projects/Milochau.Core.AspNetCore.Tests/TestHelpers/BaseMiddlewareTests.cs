using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
    public static class BaseMiddlewareTests
    {
        public static HttpContext CreateHttpContext(string method, string path)
            => CreateHttpContext(method, path, default);

        public static HttpContext CreateHttpContext(string method, string path, QueryString query)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = method;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = query;
            httpContext.Response.Body = new MemoryStream();
            return httpContext;
        }

        public static TResponse? GetResponseFromHttpContext<TResponse>(HttpContext httpContext)
        {
            httpContext.Response.Body.Position = 0;
            using var reader = new StreamReader(httpContext.Response.Body);
            var content = reader.ReadToEnd();
            return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
