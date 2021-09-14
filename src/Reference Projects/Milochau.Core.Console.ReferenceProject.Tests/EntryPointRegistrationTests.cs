using Milochau.Core.Console.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Milochau.Core.Console.ReferenceProject.Tests
{
    [TestClass]
    public partial class EntryPointRegistrationTests
    {
        [TestMethod]
        public async Task Should_CallEntryPoint_When_HostIsStarted()
        {
            // Arrange
            var businessService = new Mock<IBusinessService>();

            var hostBuilder = new HostBuilder()
                .ConfigureCoreHostBuilder<Startup, TestEntryPoint>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IBusinessService>(businessService.Object);
                });

            // Act
            await hostBuilder.RunConsoleAsync();

            // Assert
            businessService.Verify(x => x.Call(), Times.Once);
        }
    }
}
