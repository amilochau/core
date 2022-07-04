using Milochau.Core.Abstractions;
using Milochau.Core.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Milochau.Core.Tests.Infrastructure.Hosting
{
    [TestClass]
    public class StartupLoggingTests
    {
        [TestMethod("LogApplicationBuilder")]
        public void LogApplicationBuilder_When_Called()
        {
            // Given
            var services = new ServiceCollection();
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(x => x.CreateLogger("Milochau.Core.Infrastructure.Hosting")).Returns(logger.Object);
            var applicationHostEnvironment = new Mock<IApplicationHostEnvironment>();

            services.AddSingleton(loggerFactory.Object);
            services.AddSingleton(applicationHostEnvironment.Object);

            var serviceProvider = services.BuildServiceProvider();

            // When
            StartupLogging.LogApplicationInformation(serviceProvider);

            // Then
            logger.Verify(x => x.Log(It.Is<LogLevel>(ll => ll >= LogLevel.Information), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(2));
        }
    }
}
