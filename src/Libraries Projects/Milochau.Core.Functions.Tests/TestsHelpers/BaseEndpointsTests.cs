using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Milochau.Core.Console.Infrastructure.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Tests.TestsHelpers
{
    public static class BaseEndpointsTests
    {
        public static async Task<HttpClient> CreateHttpClientFromCoreAsync(IConfiguration configuration)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    if (configuration != null)
                        configurationBuilder.AddConfiguration(configuration);
                })
                .ConfigureCoreHostBuilder<TestStartup>().Build();

            await host.StartAsync();

            var server = host.Start();
            return server.CreateClient();
        }
    }
}
