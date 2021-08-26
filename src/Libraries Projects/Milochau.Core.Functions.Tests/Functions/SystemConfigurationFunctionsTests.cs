using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Tests.Functions
{
    [TestClass]
    public class SystemConfigurationFunctionsTests : BaseFunctionsTests
    {
        private Mock<IFeatureManager> featureManager;
        private IConfiguration configuration;

        private SystemConfigurationFunctions functions;

        private const string featureName = "featureName";

        [TestInitialize]
        public void Initialize()
        {
            featureManager = new Mock<IFeatureManager>();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>());
            configuration = configurationBuilder.Build();

            functions = new SystemConfigurationFunctions(featureManager.Object, configuration);
        }

        [TestMethod("Configuration - Flags")]
        public async Task Flags_Should_ReturnFlags_When_CalledAsync()
        {
            // Given
            var httpRequestData = CreateHttpRequestData("get", "/api/system/configuration/flags");
            featureManager.Setup(x => x.GetFeatureNamesAsync()).Returns(GetTestFeaturesAsync());
            featureManager.Setup(x => x.IsEnabledAsync(featureName)).ReturnsAsync(true);

            // When
            var httpResponseData = await functions.GetFlagsAsync(httpRequestData);

            // Then
            Assert.IsNotNull(httpResponseData);
            var response = GetResponseAsJson<FlagsResponse>(httpResponseData, HttpStatusCode.OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Features.Count);
            Assert.IsNotNull(response.Features.First());
            Assert.IsTrue(response.Features.First().Enabled);
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
