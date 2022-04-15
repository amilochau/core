using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {

        }

        private static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
        {

        }

        private static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            // Register other projects
        }
    }
}
