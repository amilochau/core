using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Infrastructure.Hosting;

namespace Milochau.Core.AspNetCore.Infrastructure.Hosting
{
    /// <summary>Extensions for <see cref="IHostBuilder"/></summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures core host defaults, using <see cref="WebHostBuilderExtensions.ConfigureCoreWebHostBuilder"/> and setting up <typeparamref name="TStartup"/> class</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <param name="hostBuilder">Host builder</param>
        public static IHostBuilder ConfigureCoreHostBuilder<TStartup>(this IHostBuilder hostBuilder)
            where TStartup : CoreApplicationStartup
        {
            return hostBuilder
                .ConfigureCoreConfiguration()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureCoreWebHostBuilder()
                        .UseStartup<TStartup>();
                });
        }
    }
}
