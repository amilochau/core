using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.Functions.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures core host defaults, setting up <typeparamref name="TStartup"/> class</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <param name="hostBuilder">Host builder</param>
        public static IHostBuilder ConfigureCoreHostBuilder<TStartup>(this IHostBuilder hostBuilder)
            where TStartup : CoreFunctionsStartup, new()
        {
            return hostBuilder
                .ConfigureCoreConfiguration()
                .ConfigureServices((hostContext, services) =>
                {
                    var startup = CoreFunctionsStartup.Create<TStartup>(hostContext.Configuration);
                    startup.ConfigureServices(services);
                    services.AddSingleton<CoreFunctionsStartup>(startup);
                })
                .ConfigureFunctionsWorkerDefaults((hostBuilderContext, functionsWorkerApplicationBuilder) =>
                {
                    var serviceProvider = functionsWorkerApplicationBuilder.Services.BuildServiceProvider();

                    var startup = serviceProvider.GetRequiredService<CoreFunctionsStartup>();

                    startup.Configure(serviceProvider, functionsWorkerApplicationBuilder);
                });
        }
    }
}
