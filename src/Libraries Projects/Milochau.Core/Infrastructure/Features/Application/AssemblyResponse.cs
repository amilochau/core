using System.Text.RegularExpressions;

namespace Milochau.Core.Infrastructure.Features.Application
{
    /// <summary>Response for application assembly endpoint</summary>
    public class AssemblyResponse
    {
        /// <summary>Company</summary>
        public string Company { get; set; }

        /// <summary>Configuration</summary>
        public string Configuration { get; set; }

        /// <summary>Copyright</summary>
        public string Copyright { get; set; }

        /// <summary>Description</summary>
        public string Description { get; set; }

        /// <summary>File Version</summary>
        public string FileVersion { get; set; }

        /// <summary>Informational Version</summary>
        public string InformationalVersion { get; set; }

        /// <summary>Product</summary>
        public string Product { get; set; }


        /// <summary>Is Local</summary>
        /// <remarks>Computed from <see cref="InformationalVersion"/></remarks>
        public bool IsLocal { get; set; }

        /// <summary>Build ID</summary>
        /// <remarks>Computed from <see cref="InformationalVersion"/></remarks>
        public string BuildId { get; set; }

        /// <summary>Build Source Version</summary>
        /// <remarks>Computed from <see cref="InformationalVersion"/></remarks>
        public string BuildSourceVersion { get; set; }

        /// <summary>Regular expression to know if the <see cref="InformationalVersion"/> indicates a local build</summary>
        public static readonly Regex IsLocalRegex = new Regex(@"\+local$");

        /// <summary>Regular expression to get build information from the <see cref="InformationalVersion"/></summary>
        public static readonly Regex BuildRegex = new Regex(@"\+(?<buildId>\d+)\-(?<buildSourceVersion>.+)$");
    }
}
