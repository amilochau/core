using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Milochau.Core.AspNetCore.ReferenceProject.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void CreateHostBuilder_When_Called()
        {
            // Given
            var args = new string[0];

            // When
            var hostBuilder = Program.CreateHostBuilder(args);

            // Then

            Assert.IsNotNull(hostBuilder);
        }
    }
}
