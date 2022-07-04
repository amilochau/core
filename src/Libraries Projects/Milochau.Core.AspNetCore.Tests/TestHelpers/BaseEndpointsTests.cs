using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Milochau.Core.AspNetCore.Infrastructure.Hosting;
using Milochau.Core.Infrastructure.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
    public static class BaseEndpointsTests
    {
        public static Task<HttpClient> CreateHttpClientFromCoreAsync()
            => CreateHttpClientFromCoreAsync(null);

        public static async Task<HttpClient> CreateHttpClientFromCoreAsync(IConfiguration? configuration)
        {
            var host = new HostBuilder()
                .ConfigureCoreHostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    if (configuration != null)
                    {
                        webHostBuilder = webHostBuilder.UseConfiguration(configuration);
                    }

                    webHostBuilder
                        .ConfigureCoreWebHostBuilder()
                        .UseTestServer()
                        .ConfigureServices((context, services) =>
                        {
                            var startup = new TestStartup(context.Configuration);
                            startup.ConfigureServices(services);
                        })
                        .Configure((context, app) =>
                        {
                            var startup = new TestStartup(context.Configuration);
                            startup.Configure(app);

                            app.UseRouting();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapCoreHealthEndpoints("/health");
                                endpoints.MapCoreSystemEndpoints("/system");
                            });
                        });
                }).Build();

            await host.StartAsync();

            var server = host.GetTestServer();
            return server.CreateClient();
        }

        public static async Task<HttpClient> CreateHttpClientFromCoreWithAuthenticationAsync()
        {
            var host = new HostBuilder()
                .ConfigureCoreHostBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                        .ConfigureCoreWebHostBuilder()
                        .UseTestServer()
                        .UseStartup<TestStartup>()
                        .ConfigureServices((context, services) =>
                        {
                            var startup = new TestStartup(context.Configuration);
                            startup.ConfigureServices(services);

                            services.AddAuthentication("ApiKeyAuthentication")
                                .AddScheme<ApiKeyOptions, ApiKeyAuthenticationHandler>("ApiKeyAuthentication", options =>
                                {
                                    options.ApiKey = context.Configuration["Auth:ApiKey"];
                                });
                            services.AddAuthorization();
                        })
                        .Configure((context, app) =>
                        {
                            var startup = new TestStartup(context.Configuration);
                            startup.Configure(app);

                            app.UseRouting();
                            app.UseAuthentication();
                            app.UseAuthorization();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapCoreHealthEndpoints("/health");
                                endpoints.MapCoreSystemEndpoints("/system").RequireAuthorization();
                            });
                        });
                }).Build();

            await host.StartAsync();

            var server = host.GetTestServer();
            return server.CreateClient();
        }
    }
}
