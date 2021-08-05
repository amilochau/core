using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Console.ReferenceProject.Models;

namespace Milochau.Core.Console.ReferenceProject
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
    }
}
