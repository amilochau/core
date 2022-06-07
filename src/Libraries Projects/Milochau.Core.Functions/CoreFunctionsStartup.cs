using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Infrastructure.Features;
using System;
using System.Text.Json;

namespace Milochau.Core.Functions
{
    /// <summary>Base startup class for Azure Function applications</summary>
    public abstract class CoreFunctionsStartup : CoreStartup
    {
        /// <summary>Create a new startup object</summary>
        /// <typeparam name="TStartup">Startup class</typeparam>
        /// <param name="configuration">Configuration</param>
        public static TStartup Create<TStartup>(IConfiguration configuration)
            where TStartup : CoreFunctionsStartup, new()
        {
            var startup = new TStartup();
            startup.SetHostOptions(configuration);
            return startup;
        }

        /// <summary>Configure services</summary>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            // Add token credential to be shared between connections
            services.AddSingleton<TokenCredential>(serviceProvider =>
            {
                var hostOptions = serviceProvider.GetRequiredService<IOptions<CoreHostOptions>>();
                return new DefaultAzureCredential(hostOptions.Value.Credential);
            });

            // Serialize JSON response data with camelCase
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            ConfigureHealthChecks(services);
        }

        /// <summary>Configure health checks</summary>
        protected virtual IHealthChecksBuilder ConfigureHealthChecks(IServiceCollection services)
        {
            return services.AddCoreHealthChecks(hostOptions);
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
