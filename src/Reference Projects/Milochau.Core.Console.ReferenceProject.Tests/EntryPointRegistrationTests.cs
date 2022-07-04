using Milochau.Core.Console.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Milochau.Core.Console.ReferenceProject.Tests
{
    [TestClass]
    public partial class EntryPointRegistrationTests
    {
        [TestMethod]
        public async Task Should_CallTestEntryPoint_When_HostIsStarted()
        {
            // Arrange
            var businessService = new Mock<IBusinessService>();

            var hostBuilder = new HostBuilder()
                .ConfigureConsoleCoreHostBuilder<Startup, TestEntryPoint>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IBusinessService>(businessService.Object);
                });

            // Act
            await hostBuilder.RunConsoleAsync();

            // Assert
            businessService.Verify(x => x.Call(), Times.Once);
        }

        [TestMethod]
        public async Task Should_CallEntryPoint_When_HostIsStarted()
        {
            // Arrange
            var logger = new Mock<ILogger<EntryPoint>>();

            var hostBuilder = new HostBuilder()
                .ConfigureConsoleCoreHostBuilder<Startup, EntryPoint>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ILogger<EntryPoint>>(logger.Object);
                });

            // Act
            await hostBuilder.RunConsoleAsync();

            // Assert
            logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.AtLeastOnce);
        }
    }
}
