using Milochau.Core.Infrastructure.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Milochau.Core.Tests.Infrastructure.Hosting
{
    [TestClass]
    public class ApplicationHostEnvironmentTests
    {
        private ApplicationHostEnvironment MilochauHostingEnvironment = null!;

        private const string OrganizationName = "organizationName";
        private const string ApplicationName = "applicationName";
        private const string EnvironmentName = "Production";
        private const string HostName = "prd";
        private const string RegionName = "ew1";

        [TestInitialize]
        public void Initialize()
        {
            MilochauHostingEnvironment = new ApplicationHostEnvironment(OrganizationName, ApplicationName, EnvironmentName, HostName, RegionName);
        }

        [TestMethod("MilochauHostingEnvironment - Properties")]
        public void MilochauHostingEnvironment_Should_CreateMilochauHostingEnvironment_When_Called()
        {
            Assert.AreEqual(OrganizationName, MilochauHostingEnvironment.OrganizationName);
            Assert.AreEqual(ApplicationName, MilochauHostingEnvironment.ApplicationName);
            Assert.AreEqual(EnvironmentName, MilochauHostingEnvironment.EnvironmentName);
            Assert.AreEqual(HostName, MilochauHostingEnvironment.HostName);
            Assert.AreEqual(RegionName, MilochauHostingEnvironment.RegionName);
        }

        [TestMethod("IsProduction - Called")]
        public void IsProduction_Should_ReturnTrue_When_Called()
        {
            Assert.IsTrue(MilochauHostingEnvironment.IsProduction());
        }
    }
}
