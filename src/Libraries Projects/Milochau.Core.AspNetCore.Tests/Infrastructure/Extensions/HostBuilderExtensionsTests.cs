using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Infrastructure.Extensions;
using Milochau.Core.AspNetCore.Tests.TestHelpers;

namespace Milochau.Core.AspNetCore.Tests.Infrastructure.Extensions
{
    [TestClass]
    public class HostBuilderExtensionsTests
    {
        [TestMethod("ConfigureCoreHostBuilder")]
        public void ConfigureCoreHostBuilder_When_Called()
        {
            // Given
            var hostBuilder = new HostBuilder();

            // When
            HostBuilderExtensions.ConfigureCoreHostBuilder<TestStartup>(hostBuilder);
            var host = hostBuilder.Build();

            // Then
            Assert.IsNotNull(host);
        }
    }
}
