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
    public class HostFilterTests
    {
        private Mock<IApplicationHostEnvironment> hostingEnvironment;
        private IConfiguration configuration;
        private Mock<ILogger<HostFilter>> logger;

        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        private HostFilter hostFilter;

        [TestInitialize]
        public void Initialize()
        {
            hostingEnvironment = new Mock<IApplicationHostEnvironment>();
            logger = new Mock<ILogger<HostFilter>>();

            hostFilter = new HostFilter(hostingEnvironment.Object, logger.Object);
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
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod("EvaluateAsync - Exact match")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_ExactHostMatch()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "dev" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.HostName).Returns("dev");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Match case sensitive")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_HostMatchCaseSensitive()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "DEV" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.HostName).Returns("dev");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_HostContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "dev ; dev2" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.HostName).Returns("dev");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Not exactly contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_HostNotExactlyContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "dev ; dev2" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.HostName).Returns("dev3");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod("EvaluateAsync - Not partially contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_HostNotPartiallyContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "dev2 ; dev3" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.HostName).Returns("dev");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await hostFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
