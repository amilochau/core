using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.Tests.TestHelpers;
using Moq;
using System;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Extensions
{
    [TestClass]
    public partial class ApplicationBuilderExtensionsTests
    {
        [TestMethod]
        public void UseCoreFeatures_When_ConfigurationIsSet()
        {
            // Given
            var applicationBuilder = BaseFeatureBuilderServiceTest.CreateApplicationBuilder();

            // When
            var app = AspNetCore.Infrastructure.Extensions.ApplicationBuilderExtensions.UseCoreFeatures(applicationBuilder);

            // Then
            Assert.IsNotNull(app);
        }

        [TestMethod("LogApplicationBuilder")]
        public void LogApplicationBuilder_When_Called()
        {
            // Given
            var services = new ServiceCollection();
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(x => x.CreateLogger("Milochau.Core.AspNetCore.Infrastructure")).Returns(logger.Object);
            var applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();

            services.AddSingleton(loggerFactory.Object);
            services.AddSingleton(applicationHostEnvironment.Object);

            var serviceProvider = services.BuildServiceProvider();
            var applicationBuilder = new ApplicationBuilder(serviceProvider);

            // When
            AspNetCore.Infrastructure.Extensions.ApplicationBuilderExtensions.LogApplicationBuilder(applicationBuilder);

            // Then
            logger.Verify(x => x.Log(It.Is<LogLevel>(ll => ll >= LogLevel.Information), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(2));
        }
    }
}
