using System;

namespace Milochau.Core.AspNetCore.Models
{
    /// <summary>Core services options</summary>
    public class CoreServicesOptions
    {
        /// <summary>Default section in Configuration to retrieved <see cref="CoreServicesOptions"/></summary>
        public const string DefaultConfigurationSection = "Core:Services";

        /// <summary>Telemetry options</summary>
        public TelemetryOptions Telemetry { get; set; } = new TelemetryOptions();

        /// <summary>Request localization options</summary>
        public RequestLocalizationOptions RequestLocalization { get; set; } = new RequestLocalizationOptions();
    }

    /// <summary>Telemetry options</summary>
    public class TelemetryOptions
    {
        /// <summary>Disable Adaptive Sampling</summary>
        /// <remarks>See https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling</remarks>
        public bool DisableAdaptiveSampling { get; set; }
    }

    /// <summary>Request localization options</summary>
    public class RequestLocalizationOptions
    {
        /// <summary>Enable localization</summary>
        public bool Enabled { get; set; }

        /// <summary>Default culture</summary>
        public string? DefaultCulture { get; set; }

        /// <summary>Supported (UI) cultures</summary>
        public string[] SupportedCultures { get; set; } = Array.Empty<string>();
    }
}
