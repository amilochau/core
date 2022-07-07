using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions.Models.System;
using Milochau.Core.Functions.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class SystemConfigurationFunctionsTests : BaseFunctionsTests
    {
        private IConfiguration configuration = null!;

        private SystemConfigurationFunctions functions = null!;

        private const string featureName = "featureName";

        [TestInitialize]
        public void Initialize()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>());
            configuration = configurationBuilder.Build();

            functions = new SystemConfigurationFunctions(configuration);
        }

        [TestMethod("Configuration - Providers")]
        public async Task Providers_Should_ReturnProviders_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/configuration/providers");

            // When
            var httpResponseData = await functions.GetProvidersAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<ProvidersResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Providers.Count());
            Assert.AreEqual("MemoryConfigurationProvider", response.Providers.First());
        }

        private static async IAsyncEnumerable<string> GetTestFeaturesAsync()
        {
            yield return featureName;

            await Task.CompletedTask; // to make the compiler warning go away
        }
    }
}
