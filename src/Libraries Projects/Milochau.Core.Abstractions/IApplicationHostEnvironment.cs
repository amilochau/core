namespace Milochau.Core.Abstractions
{
    /// <summary>Application host environment</summary>
    public interface IApplicationHostEnvironment
    {
        /// <summary>Organization name</summary>
        string OrganizationName { get; }

        /// <summary>Application name</summary>
        string ApplicationName { get; }

        /// <summary>Environment name</summary>
        string EnvironmentName { get; }

        /// <summary>Host name</summary>
        string HostName { get; }

        /// <summary>Region name</summary>
        string RegionName { get; }

        /// <summary>Check if the current environment is Production</summary>
        bool IsProduction();
    }
}
