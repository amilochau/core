using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.Console.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures core host defaults, setting up <typeparamref name="TStartup"/> class</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <typeparam name="TEntryPoint">Entry point class</typeparam>
        /// <param name="hostBuilder">Host builder</param>
        public static IHostBuilder ConfigureConsoleCoreHostBuilder<TStartup, TEntryPoint>(this IHostBuilder hostBuilder)
            where TStartup : CoreConsoleStartup, new()
            where TEntryPoint : CoreConsoleEntryPoint
        {
            return hostBuilder
                .ConfigureCoreHostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Register ConsoleHostedService, to handle console lifecycle
                    services.AddHostedService<ConsoleHostedService>();

                    // Configure services and entry point
                    var startup = CoreConsoleStartup.Create<TStartup>(hostContext.Configuration);
                    startup.ConfigureServices(services);
                    services.AddSingleton<CoreConsoleStartup>(startup);
                    services.AddScoped<CoreConsoleEntryPoint, TEntryPoint>();
                });
        }
    }
}
