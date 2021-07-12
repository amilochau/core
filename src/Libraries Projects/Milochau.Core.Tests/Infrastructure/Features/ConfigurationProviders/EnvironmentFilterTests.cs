using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milochau.Core.Tests.Infrastructure.Features.ConfigurationProviders
{
    [TestClass]
    public class EnvironmentFilterTests
    {
        private Mock<IApplicationHostEnvironment> hostingEnvironment;
        private IConfiguration configuration;
        private Mock<ILogger<EnvironmentFilter>> logger;

        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        private EnvironmentFilter environmentFilter;

        [TestInitialize]
        public void Initialize()
        {
            hostingEnvironment = new Mock<IApplicationHostEnvironment>();
            logger = new Mock<ILogger<EnvironmentFilter>>();

            environmentFilter = new EnvironmentFilter(hostingEnvironment.Object, logger.Object);
        }

        [TestMethod("EvaluateAsync - No valid Value")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_NoValidValue()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>());
            configuration = configurationBuilder.Build();

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await environmentFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod("EvaluateAsync - Exact match")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_ExactEnvironmentMatch()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "Development" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.EnvironmentName).Returns("Development");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await environmentFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Match case sensitive")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentMatchCaseSensitive()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "DEVELOPMENT" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.EnvironmentName).Returns("Development");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await environmentFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "Development ; Production" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.EnvironmentName).Returns("Development");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await environmentFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Not contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentNotContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "Development ; Production" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.EnvironmentName).Returns("UAT");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await environmentFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
