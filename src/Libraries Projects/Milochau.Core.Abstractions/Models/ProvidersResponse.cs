using System.Collections.Generic;

namespace Milochau.Core.Abstractions.Models
{
    /// <summary>Response for configuration providers endpoint</summary>
    public class ProvidersResponse
    {
        /// <summary>Providers</summary>
        public IEnumerable<string?> Providers { get; set; } = new List<string?>();
    }
}
