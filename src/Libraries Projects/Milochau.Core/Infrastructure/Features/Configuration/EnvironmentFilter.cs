using Milochau.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Milochau.Core.Infrastructure.Features.Configuration
{
    /// <summary>
    /// This Feature Filter is used to enable a new feature (from Azure App Configuration) only when the current environment is targetted.
    /// In Azure App Configuration UI, use a custom filter named 'Environment', and set a filter parameter named 'Value'.
    /// By example, set the 'Value' key with the 'Development,UserAcceptanceTesting' value to enable your feature in Development and UserAcceptanceTesting environments, but disable in Production.
    /// </summary>
    public class EnvironmentFilter : IFeatureFilter
    {
        private const string alias = "Environment";
        private readonly char[] separator = { ',', ';', ' ' };

        private readonly IApplicationHostEnvironment applicationHostEnvironment;
        private readonly ILogger<EnvironmentFilter> logger;

        /// <summary>Constructor</summary>
        /// <param name="applicationHostEnvironment">Application host environment</param>
        /// <param name="logger">Logger</param>
        public EnvironmentFilter(IApplicationHostEnvironment applicationHostEnvironment,
            ILogger<EnvironmentFilter> logger)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
            this.logger = logger;
        }

        /// <summary>Evaluates filter</summary>
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var environmentFilterSettings = context.Parameters.Get<EnvironmentFilterSettings>() ?? new EnvironmentFilterSettings();
            if (string.IsNullOrEmpty(environmentFilterSettings.Value))
            {
                logger.LogWarning($"The '{alias}' feature filter does not have a valid 'Value' value for feature '{context.FeatureName}'");
                return Task.FromResult(false);
            }

            var flag = environmentFilterSettings.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries).Contains(applicationHostEnvironment.EnvironmentName, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(flag);
        }
    }
}
