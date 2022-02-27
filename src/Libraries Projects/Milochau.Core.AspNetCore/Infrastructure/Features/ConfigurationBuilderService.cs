using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Routing;

namespace Milochau.Core.AspNetCore.Infrastructure.Features
{
    /// <summary>Extensions for <see cref="IApplicationBuilder"/> and <see cref="IServiceCollection"/></summary>    
    internal static class ConfigurationBuilderService
    {
        private const string defaultDisplayName = "Configuration";

        /// <summary>Adds configuration endpoints</summary>
        /// <param name="endpoints">Endpoint route builder</param>
        /// <param name="path">Path for endpoints</param>
        public static IEndpointConventionBuilder MapCoreConfiguration(this IEndpointRouteBuilder endpoints, string path)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
               .UseMiddleware<SystemConfigurationMiddleware>()
               .Build();

            return endpoints.Map(path + "/configuration/{*sub}", pipeline).WithDisplayName(defaultDisplayName);
        }
    }
}
