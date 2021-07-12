using System.Collections.Generic;

namespace Milochau.Core.Infrastructure.Features.Configuration
{
    /// <summary>Response for configuration flags endpoint</summary>
    public class FlagsResponse
    {
        /// <summary>Features</summary>
        public ICollection<FeatureDetails> Features { get; set; } = new List<FeatureDetails>();
    }

    /// <summary>Feature details</summary>
    public class FeatureDetails
    {
        /// <summary>Name</summary>
        public string Name { get; set; }

        /// <summary>Enabled</summary>
        public bool Enabled { get; set; }
    }
}
