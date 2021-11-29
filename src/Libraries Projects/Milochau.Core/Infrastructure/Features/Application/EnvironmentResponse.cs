using Milochau.Core.Abstractions;
using System;
using System.Globalization;

namespace Milochau.Core.Infrastructure.Features.Application
{
    /// <summary>Response for application environment endpoint</summary>
    public class EnvironmentResponse
    {
        /// <summary>Organization name</summary>
        public string OrganizationName { get; set; }

        /// <summary>Application name</summary>
        public string ApplicationName { get; set; }

        /// <summary>Environment name</summary>
        public string EnvironmentName { get; set; }

        /// <summary>Host name</summary>
        public string HostName { get; set; }

        /// <summary>Region name</summary>
        public string RegionName { get; set; }


        /// <summary>Machine name</summary>
        public string MachineName { get; set; } = Environment.MachineName;

        /// <summary>Processor count</summary>
        public int ProcessorCount { get; set; } = Environment.ProcessorCount;

        /// <summary>OS version</summary>
        public string OSVersion { get; set; } = Environment.OSVersion.ToString();

        /// <summary>CLR version</summary>
        public string ClrVersion { get; set; } = Environment.Version.ToString();

        /// <summary>OS is 64 bits</summary>
        public bool Is64BitOperatingSystem { get; set; } = Environment.Is64BitOperatingSystem;

        /// <summary>Process is 64 bits</summary>
        public bool Is64BitProcess { get; set; } = Environment.Is64BitProcess;


        /// <summary>Local time zone information</summary>
        public string LocalTimeZone { get; set; } = TimeZoneInfo.Local.Id;

        /// <summary>UTC time zone information</summary>
        public string UtcTimeZone { get; set; } = TimeZoneInfo.Utc.Id;


        /// <summary>Current culture</summary>
        public string CurrentCulture { get; set; } = CultureInfo.CurrentCulture.Name;

        /// <summary>Default constructor</summary>
        public EnvironmentResponse()
        {
        }

        /// <summary>Constructor</summary>
        public EnvironmentResponse(IApplicationHostEnvironment applicationHostEnvironment)
        {
            OrganizationName = applicationHostEnvironment.OrganizationName;
            ApplicationName = applicationHostEnvironment.ApplicationName;
            EnvironmentName = applicationHostEnvironment.EnvironmentName;
            HostName = applicationHostEnvironment.HostName;
            RegionName = applicationHostEnvironment.RegionName;
        }
    }
}
