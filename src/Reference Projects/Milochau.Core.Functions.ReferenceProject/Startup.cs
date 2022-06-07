using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Milochau.Core.Functions.ReferenceProject
{
    public class Startup : CoreFunctionsStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

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
