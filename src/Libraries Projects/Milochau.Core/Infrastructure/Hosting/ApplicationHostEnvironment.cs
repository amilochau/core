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
        /// <param name="applicationName">Application name</param>
        /// <param name="environmentName">Environment name</param>
        /// <param name="hostName">Host name</param>
        public ApplicationHostEnvironment(string applicationName, string environmentName, string hostName)
        {
            EnvironmentName = environmentName;
            ApplicationName = applicationName;
            HostName = hostName;
        }

        /// <summary>
        /// Gets or sets the name of the application. This property is automatically set by the host to the assembly containing
        /// the application entry point.
        /// </summary>
        public string EnvironmentName { get; }

        /// <summary>
        /// Gets or sets the name of the environment. The host automatically sets this property to the value of the
        /// of the "environment" key as specified in configuration.
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// Gets or sets the name of the host. The host automatically sets this property to the value of the
        /// of the "host" key as specified in configuration.
        /// </summary>
        public string HostName { get; }

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
