using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Features.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milochau.Core.Tests.Infrastructure.Features.ConfigurationProviders
{
    [TestClass]
    public class ApplicationFilterTests
    {
        private Mock<IApplicationHostEnvironment> hostingEnvironment;
        private IConfiguration configuration;
        private Mock<ILogger<ApplicationFilter>> logger;

        private readonly ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

        private ApplicationFilter applicationFilter;

        [TestInitialize]
        public void Initialize()
        {
            hostingEnvironment = new Mock<IApplicationHostEnvironment>();
            logger = new Mock<ILogger<ApplicationFilter>>();

            applicationFilter = new ApplicationFilter(hostingEnvironment.Object, logger.Object);
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
            var result = await applicationFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod("EvaluateAsync - Exact match")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_ExactEnvironmentMatch()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "emails" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.ApplicationName).Returns("emails");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await applicationFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Match case sensitive")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentMatchCaseSensitive()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "EMAILS" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.ApplicationName).Returns("EMAILS");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await applicationFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "emails; content" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.ApplicationName).Returns("emails");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await applicationFilter.EvaluateAsync(context);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod("EvaluateAsync - Not contained")]
        public async Task EvaluateAsync_Should_ReturnFalse_When_EnvironmentNotContained()
        {
            // Arrange
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Value", "emails ; content" }
            });
            configuration = configurationBuilder.Build();
            hostingEnvironment.SetupGet(x => x.ApplicationName).Returns("cv");

            var context = new FeatureFilterEvaluationContext
            {
                Parameters = configuration
            };

            // Act
            var result = await applicationFilter.EvaluateAsync(context);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
