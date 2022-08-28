using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Infrastructure.Hosting;
using System;

namespace Milochau.Core.Functions.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures core host defaults, setting up <typeparamref name="TStartup"/> class</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <param name="hostBuilder">Host builder</param>
        /// <param name="configure">A delegate that will be invoked to configure the provided <see cref="IFunctionsWorkerApplicationBuilder"/>.</param>
        public static IHostBuilder ConfigureFunctionsCoreHostBuilder<TStartup>(this IHostBuilder hostBuilder, Action<IFunctionsWorkerApplicationBuilder>? configure = null)
            where TStartup : CoreFunctionsStartup, new()
        {
            return hostBuilder
                .ConfigureFunctionsWorkerDefaults(workerApplication =>
                {
                    if (configure != null)
                    {
                        configure.Invoke(workerApplication);
                    }
                })
                .ConfigureCoreHostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var startup = CoreFunctionsStartup.Create<TStartup>(hostContext.Configuration);
                    startup.ConfigureServices(services);

                    var serviceProvider = services.BuildServiceProvider();

                    startup.Configure(serviceProvider);
                });
        }
    }
}
