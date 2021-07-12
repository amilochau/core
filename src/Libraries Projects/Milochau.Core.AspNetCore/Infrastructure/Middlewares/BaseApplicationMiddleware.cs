using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    internal static class BaseApplicationMiddleware
    {
        /// <summary>Writes an error</summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="message">Message to write in the response</param>
        /// <remarks>A status code <see cref="StatusCodes.Status404NotFound"/> is used</remarks>
        public static Task WriteErrorAsTextAsync(HttpContext httpContext, string message)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            httpContext.Response.ContentType = Keys.TextResponseType;
            return httpContext.Response.WriteAsync(message);
        }

        /// <summary>Serialize and write a response as JSON</summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="response">Response to serialize and write</param>
        /// <remarks>A status code <see cref="StatusCodes.Status200OK"/> is used</remarks>
        public static Task WriteResponseAsJsonAsync<TResponse>(HttpContext httpContext, TResponse response)
        {
            return httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
