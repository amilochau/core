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
    /// This Feature Filter is used to enable a new feature (from Azure App Configuration) only when the current host is targetted.
    /// In Azure App Configuration UI, use a custom filter named 'Host', and set a filter parameter named 'Value'.
    /// By example, set the 'Value' key with the 'dev,dev2' value to enable your feature in dev1 and dev2, but disable in dev3, dev4, uat...
    /// </summary>
    public class HostFilter : IFeatureFilter
    {
        private const string alias = "Host";
        private readonly char[] separator = { ',', ';', ' ' };

        private readonly IApplicationHostEnvironment applicationHostEnvironment;
        private readonly ILogger<HostFilter> logger;

        /// <summary>Constructor</summary>
        /// <param name="applicationHostEnvironment">Application host environment</param>
        /// <param name="logger">Logger</param>
        public HostFilter(IApplicationHostEnvironment applicationHostEnvironment,
            ILogger<HostFilter> logger)
        {
            this.applicationHostEnvironment = applicationHostEnvironment;
            this.logger = logger;
        }

        /// <summary>Evaluates filter</summary>
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var hostFilterSettings = context.Parameters.Get<HostFilterSettings>() ?? new HostFilterSettings();
            if (string.IsNullOrEmpty(hostFilterSettings.Value))
            {
                logger.LogWarning($"The '{alias}' feature filter does not have a valid 'Value' value for feature '{context.FeatureName}'");
                return Task.FromResult(false);
            }

            var flag = hostFilterSettings.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries).Contains(applicationHostEnvironment.HostName, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(flag);
        }
    }
}
