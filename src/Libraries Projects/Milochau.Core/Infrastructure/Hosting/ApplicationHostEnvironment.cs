using Milochau.Core.Abstractions;
using System;

namespace Milochau.Core.Infrastructure.Hosting
{
    /// <summary>Application host environment</summary>
    public class ApplicationHostEnvironment : IApplicationHostEnvironment
    {
        /// <summary>Development environment name</summary>
        public const string DevelopmentEnvironmentName = "Development";

        /// <summary>Production environment name</summary>
        public const string ProductionEnvironmentName = "Production";

        /// <summary>Local host name</summary>
        public const string LocalHostName = "local";

        /// <summary>Constructor</summary>
        /// <param name="organizationName">Organization name</param>
        /// <param name="applicationName">Application name</param>
        /// <param name="environmentName">Environment name</param>
        /// <param name="hostName">Host name</param>
        /// <param name="regionName">Host name</param>
        public ApplicationHostEnvironment(string organizationName, string applicationName, string environmentName, string hostName, string regionName)
        {
            OrganizationName = organizationName;
            EnvironmentName = environmentName;
            ApplicationName = applicationName;
            HostName = hostName;
            RegionName = regionName;
        }

        /// <summary>The name of the organization</summary>
        public string OrganizationName { get; set; }

        /// <summary>The name of the application</summary>
        public string ApplicationName { get; }

        /// <summary>The name of the environment</summary>
        public string EnvironmentName { get; }

        /// <summary>The name of the host</summary>
        public string HostName { get; }

        /// <summary>The name of the region</summary>
        public string RegionName { get; }

        /// <summary>Check if the current environment is Production</summary>
        public bool IsProduction()
        {
            return string.Equals(
                EnvironmentName,
                ProductionEnvironmentName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
