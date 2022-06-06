using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Milochau.Core.Abstractions.Exceptions;
using Milochau.Core.Functions.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Infrastructure.Middelwares
{
    /// <summary>Middleware to handle request exception</summary>
    internal sealed class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
    {
        /// <summary>Invoke middleware</summary>
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException)
            {
                var httpRequestData = await context.GetHttpRequestDataAsync();
                if (httpRequestData != null)
                {
                    context.GetInvocationResult().Value = httpRequestData.WriteEmptyResponseAsync(HttpStatusCode.NotFound);
                }
            }
        }
    }
}
