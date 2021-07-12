using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Functions.Functions;
using Milochau.Core.Infrastructure.Features.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
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
            var httpContext = CreateHttpContext("get", "/api/system/configuration/flags");
            featureManager.Setup(x => x.GetFeatureNamesAsync()).Returns(GetTestFeaturesAsync());
            featureManager.Setup(x => x.IsEnabledAsync(featureName)).ReturnsAsync(true);

            // When
            var result = await functions.FlagsAsync(httpContext.Request);

            // Then
            Assert.IsNotNull(result);
            var response = GetResponseFromActionResult<FlagsResponse>(result, StatusCodes.Status200OK);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Features.Count);
            Assert.IsNotNull(response.Features.First());
            Assert.IsTrue(response.Features.First().Enabled);
        }

        [TestMethod("Configuration - Providers")]
        public void Providers_Should_ReturnProviders_When_Called()
        {
            // Given
            var httpContext = CreateHttpContext("get", "/api/system/configuration/providers");

            // When
            var result = functions.Providers(httpContext.Request);

            // Then
            Assert.AreEqual(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            var response = GetResponseFromActionResult<ProvidersResponse>(result, StatusCodes.Status200OK);
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
