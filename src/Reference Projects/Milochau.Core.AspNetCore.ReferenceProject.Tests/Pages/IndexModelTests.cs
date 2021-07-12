using Milochau.Core.AspNetCore.ReferenceProject.Models;
using Milochau.Core.AspNetCore.ReferenceProject.Pages;
using Milochau.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Milochau.Core.AspNetCore.Models;

namespace Milochau.Core.AspNetCore.ReferenceProject.Tests.Pages
{
    [TestClass]
    public class IndexModelTests
    {
        private Mock<IOptionsSnapshot<TestOptions>> testOptionsMock;
        private Mock<IOptions<CoreHostOptions>> coreHostOptionsMock;
        private Mock<IOptions<CoreServicesOptions>> coreServicesOptionsMock;
        private Mock<IFeatureManager> featureManager;
        private IndexModel indexModel;

        private readonly TestOptions testOptions = new TestOptions();
        private readonly CoreHostOptions coreHostOptions = new CoreHostOptions();
        private readonly CoreServicesOptions coreServicesOptions = new CoreServicesOptions();

        [TestInitialize]
        public void Initialize()
        {
            testOptionsMock = new Mock<IOptionsSnapshot<TestOptions>>();
            testOptionsMock.Setup(x => x.Value).Returns(testOptions);
            coreHostOptionsMock = new Mock<IOptions<CoreHostOptions>>();
            coreHostOptionsMock.Setup(x => x.Value).Returns(coreHostOptions);
            coreServicesOptionsMock = new Mock<IOptions<CoreServicesOptions>>();
            coreServicesOptionsMock.Setup(x => x.Value).Returns(coreServicesOptions);
            featureManager = new Mock<IFeatureManager>();

            indexModel = new IndexModel(testOptionsMock.Object, coreHostOptionsMock.Object, coreServicesOptionsMock.Object, featureManager.Object);
        }

        [TestMethod]
        public async Task Options_When_CreatedAsync()
        {
            // Given
            featureManager.Setup(x => x.IsEnabledAsync(nameof(FeatureFlags.ReferenceProjectTest))).Returns(Task.FromResult(true));

            // When
            await indexModel.OnGetAsync();

            // Then
            Assert.IsNotNull(indexModel.TestOptions);
            Assert.IsNotNull(indexModel.CoreHostOptions);
            Assert.IsTrue(indexModel.ReferenceProjectTest);
        }
    }
}
