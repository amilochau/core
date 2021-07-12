using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/>, specific to Cache management</summary>
    internal static class CacheBuilderService
    {
        private const string defaultDisplayName = "Cache";

        /// <summary>Adds cache endpoints</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreCache(this IEndpointRouteBuilder endpoints, string path)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<SystemCacheMiddleware>()
                .Build();

            return endpoints.Map(path + "/cache/{*sub}", pipeline).WithDisplayName(defaultDisplayName);
        }
    }
}
