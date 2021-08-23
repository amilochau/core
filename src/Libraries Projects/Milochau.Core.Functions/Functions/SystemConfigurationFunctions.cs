using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Milochau.Core.Infrastructure.Features.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Functions
{
    /// <summary>System Functions to expose configuration management</summary>
    public class SystemConfigurationFunctions
    {
        private readonly IFeatureManager featureManager;
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public SystemConfigurationFunctions(IFeatureManager featureManager,
            IConfiguration configuration)
        {
            this.featureManager = featureManager;
            this.configuration = configuration;
        }

        /// <summary>Get feature flags state</summary>
        [Function("System-Configuration-Flags")]
        public async Task<IActionResult> FlagsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/configuration/flags")] HttpRequest request)
        {
            var response = new FlagsResponse();
            await foreach (var featureName in featureManager.GetFeatureNamesAsync())
            {
                var enabled = await featureManager.IsEnabledAsync(featureName);
                response.Features.Add(new FeatureDetails { Name = featureName, Enabled = enabled });
            }

            return new OkObjectResult(response);
        }

        /// <summary>Get configuration providers</summary>
        [Function("System-Configuration-Providers")]
        public IActionResult Providers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "system/configuration/providers")] HttpRequest request)
        {
            var configurationRoot = configuration as ConfigurationRoot;
            var response = new ProvidersResponse
            {
                Providers = configurationRoot.Providers.Select(x => x.ToString())
            };

            return new OkObjectResult(response);
        }
    }
}
