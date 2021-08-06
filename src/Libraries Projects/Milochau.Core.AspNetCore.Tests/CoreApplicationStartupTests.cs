using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Models;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Core.AspNetCore.Tests
{
    [TestClass]
    public class CoreApplicationStartupTests
    {
        private const string endpointCheckName = "Endpoint";

        [TestMethod("ConfigureServices - All services registred")]
        public void ConfigureServices_When_Called()
        {
            // Given
            var services = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ASPNETCORE_APPCONFIG_ENDPOINT", "https://" },
                { "ASPNETCORE_KEYVAULT_VAULT", "https://xxx.vault.azure.net" }
            });
            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            var startup = new TestStartup(configuration);

            // When
            startup.ConfigureServices(services);

            // Then
            var sp = services.BuildServiceProvider();

            Assert.IsNotNull(sp.GetRequiredService<IOptions<CoreHostOptions>>());
            Assert.IsNotNull(sp.GetRequiredService<IOptions<CoreServicesOptions>>());

            Assert.IsTrue(services.Any(x => x.ServiceType.Name == "HealthCheckService"));

            var options = sp.GetService<IOptions<HealthCheckServiceOptions>>();

            Assert.IsNotNull(options.Value);
            Assert.IsNotNull(options.Value.Registrations);
            Assert.AreEqual(2, options.Value.Registrations.Count);
            var firstRegistration = options.Value.Registrations.ElementAt(0);
            Assert.IsNotNull(firstRegistration);
            Assert.AreEqual(endpointCheckName, firstRegistration.Name);
        }
    }
}
