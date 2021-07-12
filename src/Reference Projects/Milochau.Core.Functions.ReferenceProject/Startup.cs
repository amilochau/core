using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Milochau.Core.Functions.ReferenceProject;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Milochau.Core.Functions.ReferenceProject
{
    public class Startup : CoreFunctionsStartup
    {
        protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services);
            RegisterServices(services);
            RegisterDataAccess(services);
        }

        private void RegisterOptions(IServiceCollection services)
        {
            // Register options here
        }

        private void RegisterServices(IServiceCollection services)
        {
            // Register services here
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            // Register data access here
        }
    }
}
