using Milochau.Core.Infrastructure.Hosting;
using Milochau.Core.Models;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Milochau.Core.Tests.Infrastructure.Hosting
{
    [TestClass]
    public class AppConfigurationRegistrationTests
    {
        private const string defaultApplicationName = "App";
        private const string defaultEnvironmentName = "Production";
        private const string defaultAppConfigKey = "Shared";

        [TestMethod]
        public void ConfigureAzureAppConfiguration_When_CalledWithEndpoint_And_AllFeaturesAreDisabled()
        {
            // Given
            const string appConfigEndpoint = "https://XXXX.azconfig.io";
            const string appConfigConnectionString = "";
            var appConfigOptions = new AzureAppConfigurationOptions();
            var coreHostOptions = new CoreHostOptions
            {
                AppConfig = new AppConfigurationOptions
                {
                    Endpoint = appConfigEndpoint,
                    ConnectionString = appConfigConnectionString
                }
            };

            // When
            AppConfigurationRegistration.ConfigureAzureAppConfiguration(appConfigOptions, coreHostOptions);

            // Then
        }

        [TestMethod]
        public void ConfigureAzureAppConfiguration_When_CalledWithConnectionString_And_AllFeaturesAreDisabled()
        {
            // Given
            const string appConfigEndpoint = "";
            const string appConfigConnectionString = "Endpoint=https://XXXX.azconfig.io";
            var appConfigOptions = new AzureAppConfigurationOptions();
            var coreHostOptions = new CoreHostOptions
            {
                AppConfig = new AppConfigurationOptions
                {
                    Endpoint = appConfigEndpoint,
                    ConnectionString = appConfigConnectionString
                }
            };

            // When
            AppConfigurationRegistration.ConfigureAzureAppConfiguration(appConfigOptions, coreHostOptions);

            // Then
        }

        [TestMethod]
        public void ConfigureKeyLabels_When_Called()
        {
            // Given
            var appConfigOptions = new AzureAppConfigurationOptions();
            var coreHostOptions = new CoreHostOptions
            {
                Application = new ApplicationOptions
                {
                    ApplicationName = defaultApplicationName,
                    EnvironmentName = defaultEnvironmentName
                }
            };

            // When
            AppConfigurationRegistration.ConfigureKeyLabels(appConfigOptions, coreHostOptions);

            // Then
            Assert.AreEqual(4, appConfigOptions.KeyValueSelectors.Count());
            var selectors = appConfigOptions.KeyValueSelectors.ToArray();
            // Order of selectors is important
            Assert.IsTrue(selectors[0].KeyFilter == $"{defaultAppConfigKey}*");
            Assert.IsTrue(selectors[0].LabelFilter == "\0");
            Assert.IsTrue(selectors[1].KeyFilter == $"{defaultApplicationName}*");
            Assert.IsTrue(selectors[1].LabelFilter == "\0");
            Assert.IsTrue(selectors[2].KeyFilter == $"{defaultAppConfigKey}*");
            Assert.IsTrue(selectors[2].LabelFilter == defaultEnvironmentName);
            Assert.IsTrue(selectors[3].KeyFilter == $"{defaultApplicationName}*");
            Assert.IsTrue(selectors[3].LabelFilter == defaultEnvironmentName);
        }

        [TestMethod]
        public void ConfigureRefresh_When_Enabled()
        {
            // Given
            var appConfigOptions = new AzureAppConfigurationOptions();
            var coreHostOptions = new CoreHostOptions()
            {
                AppConfig = new AppConfigurationOptions
                {
                    SentinelKey = "Key"
                }
            };

            // When
            AppConfigurationRegistration.ConfigureRefresh(appConfigOptions, coreHostOptions);

            // Then
            // No exception - Only private members are changed
        }
        [TestMethod]
        public void ConfigureFeatureFlags_When_Called()
        {
            // Given
            var appConfigOptions = new AzureAppConfigurationOptions();
            var coreHostOptions = new CoreHostOptions();

            // When
            AppConfigurationRegistration.ConfigureFeatureFlags(appConfigOptions, coreHostOptions);

            // Then
            Assert.AreEqual(1, appConfigOptions.KeyValueSelectors.Count());
            var keyValueSelector = appConfigOptions.KeyValueSelectors.First();
            // Feature Flags adds a Key-Value selector with defiend KeyFilter
            Assert.AreEqual(".appconfig.featureflag/*", keyValueSelector.KeyFilter);
        }
    }
}
