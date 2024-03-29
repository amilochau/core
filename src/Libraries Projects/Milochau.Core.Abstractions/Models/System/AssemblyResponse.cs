﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace Milochau.Core.Abstractions.Models.System
{
    /// <summary>Response for application assembly endpoint</summary>
    public class AssemblyResponse
    {
        /// <summary>Company</summary>
        public string? Company { get; set; }

        /// <summary>Configuration</summary>
        public string? Configuration { get; set; }

        /// <summary>Copyright</summary>
        public string? Copyright { get; set; }

        /// <summary>Description</summary>
        public string? Description { get; set; }

        /// <summary>File Version</summary>
        public string? FileVersion { get; set; }

        /// <summary>Informational Version</summary>
        public string? InformationalVersion { get; set; }

        /// <summary>Product</summary>
        public string? Product { get; set; }


        /// <summary>Is Local</summary>
        public bool IsLocal { get; set; } = false!;

        /// <summary>Build ID</summary>
        public string BuildId { get; set; } = null!;

        /// <summary>Build Source Version</summary>
        public string BuildSourceVersion { get; set; } = null!;

        /// <summary>Default constructor</summary>
        public AssemblyResponse()
        {

        }

        /// <summary>Constructor</summary>
        public AssemblyResponse(Assembly assembly)
        {
            Company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            Configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            Product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

            IsLocal = IsLocalRegex.IsMatch(InformationalVersion ?? string.Empty);
            BuildId = BuildRegex.Match(InformationalVersion ?? string.Empty).Groups[1].Value;
            BuildSourceVersion = BuildRegex.Match(InformationalVersion ?? string.Empty).Groups[2].Value;
        }

        /// <summary>Regular expression to know if the <see cref="InformationalVersion"/> indicates a local build</summary>
        private static readonly Regex IsLocalRegex = new Regex(@"\+local$");

        /// <summary>Regular expression to get build information from the <see cref="InformationalVersion"/></summary>
        private static readonly Regex BuildRegex = new Regex(@"\+(?<buildId>\d+)\-(?<buildSourceVersion>.+)$");
    }
}
