using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
    public static class BaseFeatureBuilderServiceTest
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddRouting();
            services.AddLogging();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection();
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            return services;
        }

        public static IApplicationBuilder CreateApplicationBuilder()
        {
            var services = new ServiceCollection();
            return CreateApplicationBuilder(services);
        }

        public static IApplicationBuilder CreateApplicationBuilder(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>());
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddOptions();

            var startup = new TestStartup(configuration);
            startup.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            return new ApplicationBuilder(serviceProvider);
        }
    }
}
