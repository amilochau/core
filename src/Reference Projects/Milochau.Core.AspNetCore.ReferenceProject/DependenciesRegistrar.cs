using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.AspNetCore.ReferenceProject.Models;

namespace Milochau.Core.AspNetCore.ReferenceProject
{
    /// <summary>Dependencies registrar</summary>
    public static class DependenciesRegistrar
    {
        /// <summary>Register services</summary>
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            RegisterDependencies(services, configuration);
            RegisterServices(services, configuration);
            RegisterOptions(services, configuration);
            RegisterHealthChecks(services, configuration);
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        private static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TestOptions>(configuration.GetSection("Test"));
        }

        private static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            // Register other projects
        }

        private static void RegisterHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
            // Default endpoint and Key Vault are registred by AssurWare.Core. Register here your application specific dependencies.
        }
    }
}
