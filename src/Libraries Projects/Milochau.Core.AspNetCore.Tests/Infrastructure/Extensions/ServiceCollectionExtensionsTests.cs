using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Core.AspNetCore.Tests.Extensions
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        private const string endpointCheckName = "Endpoint";

        [TestMethod]
        public void AddCoreFeatures_When_Called()
        {
            // Given
            var serviceCollection = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ASPNETCORE_APPCONFIG_ENDPOINT", "https://" },
                { "ASPNETCORE_KEYVAULT_VAULT", "https://xxx.vault.azure.net" }
            });
            var configuration = configurationBuilder.Build();

            // When
            AspNetCore.Infrastructure.Extensions.ServiceCollectionExtensions.AddCoreFeatures(serviceCollection, configuration);

            // Then
            Assert.IsTrue(serviceCollection.Any(x => x.ServiceType.Name == "HealthCheckService"));

            var sp = serviceCollection.BuildServiceProvider();
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
