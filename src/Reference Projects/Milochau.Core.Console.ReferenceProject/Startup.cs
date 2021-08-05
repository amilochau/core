using Microsoft.Extensions.DependencyInjection;

namespace Milochau.Core.Console.ReferenceProject
{
    public class Startup : CoreConsoleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            DependenciesRegistrar.Register(services, configuration);
        }
    }
}
