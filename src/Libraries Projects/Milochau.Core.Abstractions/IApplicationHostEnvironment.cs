namespace Milochau.Core.Abstractions
{
    /// <summary>Application host environment</summary>
    public interface IApplicationHostEnvironment
    {
        /// <summary>Application name</summary>
        /// <remarks>The host automatically sets this property to the value of the "application" key as specified in configuration.</remarks>
        string ApplicationName { get; }

        /// <summary>Environment name</summary>
        /// <remarks>The host automatically sets this property to the value of the "environment" key as specified in configuration.</remarks>
        string EnvironmentName { get; }

        /// <summary>Host name</summary>
        /// <remarks>The host automatically sets this property to the value of the "host" key as specified in configuration.</remarks>
        string HostName { get; }

        /// <summary>Check if the current environment is Production</summary>
        bool IsProduction();
    }
}
