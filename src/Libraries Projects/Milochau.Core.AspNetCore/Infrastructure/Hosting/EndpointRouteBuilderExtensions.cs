using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Milochau.Core.AspNetCore.Infrastructure.Features;

namespace Milochau.Core.AspNetCore.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IEndpointRouteBuilder"/></summary>
    public static class EndpointRouteBuilderExtensions
    {
        private const string healthDefaultPath = "/api/health";
        private const string SystemDefaultPath = "/api/system";

        /// <summary>Map health checks endpoints exposed</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        public static IEndpointConventionBuilder MapCoreHealthEndpoints(this IEndpointRouteBuilder endpoints)
            => MapCoreHealthEndpoints(endpoints, healthDefaultPath);

        /// <summary>Map health checks endpoints exposed</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreHealthEndpoints(this IEndpointRouteBuilder endpoints, string path)
        {
            return new MapActionEndpointConventionBuilder(
                endpoints.MapCoreDefaultHealthChecks(path),
                endpoints.MapCoreLightHealthChecks(path)
            );
        }

        /// <summary>Map system endpoints exposed</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        public static IEndpointConventionBuilder MapCoreSystemEndpoints(this IEndpointRouteBuilder endpoints)
            => MapCoreSystemEndpoints(endpoints, SystemDefaultPath);

        /// <summary>Map system endpoints exposed</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreSystemEndpoints(this IEndpointRouteBuilder endpoints, string path)
        {
            return new MapActionEndpointConventionBuilder(
                endpoints.MapCoreApplication(path),
                endpoints.MapCoreCache(path),
                endpoints.MapCoreConfiguration(path)
            );
        }
    }
}
