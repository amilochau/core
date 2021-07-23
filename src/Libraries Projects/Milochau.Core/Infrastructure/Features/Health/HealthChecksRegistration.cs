﻿using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Milochau.Core.Models;
using System;

namespace Milochau.Core.Infrastructure.Features.Health
{
    /// <summary>Health checks registration</summary>
    public static class HealthChecksRegistration
    {
        private const string azureKeyVaultName = "Key Vault";

        /// <summary>Tag for light checks</summary>
        public const string LightTag = "light";

        /// <summary>Register health checks into <paramref name="services"/></summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostOptions">Host options, see <see cref="CoreHostOptions"/></param>
        public static IHealthChecksBuilder RegisterHealthChecks(IServiceCollection services, CoreHostOptions hostOptions)
        {
            IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

            // Add default endpoint health check
            healthChecksBuilder.AddCheck("Endpoint", () => HealthCheckResult.Healthy(), new[] { LightTag });

            // Add Azure Key Vault health check
            if (!string.IsNullOrEmpty(hostOptions.KeyVault.Vault))
            {
                var credential = new DefaultAzureCredential(hostOptions.Credential);
                healthChecksBuilder.AddAzureKeyVault(new Uri(hostOptions.KeyVault.Vault), credential, null, azureKeyVaultName);
            }

            return healthChecksBuilder;
        }
    }
}
