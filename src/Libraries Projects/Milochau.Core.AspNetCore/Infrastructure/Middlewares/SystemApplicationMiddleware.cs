using Microsoft.AspNetCore.Http;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Application;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Infrastructure.Middlewares
{
    /// <summary>Middleware that exposes an application information response with a URL endpoint</summary>
    internal class SystemApplicationMiddleware
    {
        private readonly IApplicationHostEnvironment applicationHostEnvironment;

        /// <summary>Constructor</summary>
        public SystemApplicationMiddleware(RequestDelegate _,
            IApplicationHostEnvironment applicationHostEnvironment)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
        }

        /// <summary>Processes a request</summary>
        /// <param name="httpContext">HTTP context</param>
        public Task InvokeAsync(HttpContext httpContext)
        {
            return httpContext.Request.Method switch
            {
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/assembly", StringComparison.OrdinalIgnoreCase) => AssemblyInformationAsync(httpContext),
                Keys.GetMethod when httpContext.Request.Path.Value.EndsWith("/environment", StringComparison.OrdinalIgnoreCase) => EnvironmentAsync(httpContext),
                _ => BaseApplicationMiddleware.WriteErrorAsTextAsync(httpContext, Keys.EndpointRouteNotFoundMessage)
            };
        }

        private static Task AssemblyInformationAsync(HttpContext httpContext)
        {
            var assembly = Assembly.GetEntryAssembly();
            var response = new AssemblyResponse(assembly);

            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }

        private Task EnvironmentAsync(HttpContext httpContext)
        {
            var response = new EnvironmentResponse(applicationHostEnvironment);
            return BaseApplicationMiddleware.WriteResponseAsJsonAsync(httpContext, response);
        }
    }
}
