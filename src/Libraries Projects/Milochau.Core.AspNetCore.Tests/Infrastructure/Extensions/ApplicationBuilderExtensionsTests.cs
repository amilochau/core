using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.AspNetCore.Tests.TestHelpers;

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
    }
}
