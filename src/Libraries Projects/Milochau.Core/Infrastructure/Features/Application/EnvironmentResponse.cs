namespace Milochau.Core.Infrastructure.Features.Application
{
    /// <summary>Response for application environment endpoint</summary>
    public class EnvironmentResponse
    {
        /// <summary>Application name</summary>
        public string ApplicationName { get; set; }

        /// <summary>Environment name</summary>
        public string EnvironmentName { get; set; }

        /// <summary>Host name</summary>
        public string HostName { get; set; }


        /// <summary>Machine name</summary>
        public string MachineName { get; set; }

        /// <summary>Processor count</summary>
        public int ProcessorCount { get; set; }

        /// <summary>OS version</summary>
        public string OSVersion { get; set; }

        /// <summary>CLR version</summary>
        public string ClrVersion { get; set; }

        /// <summary>OS is 64 bits</summary>
        public bool Is64BitOperatingSystem { get; set; }

        /// <summary>Process is 64 bits</summary>
        public bool Is64BitProcess { get; set; }


        /// <summary>Local time zone information</summary>
        public string LocalTimeZone { get; set; }

        /// <summary>UTC time zone information</summary>
        public string UtcTimeZone { get; set; }
        
        
        /// <summary>Current culture</summary>
        public string CurrentCulture { get; set; }
    }
}
