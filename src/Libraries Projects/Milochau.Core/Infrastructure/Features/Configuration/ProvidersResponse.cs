using System.Collections.Generic;

namespace Milochau.Core.Infrastructure.Features.Configuration
{
    /// <summary>Response for configuration providers endpoint</summary>
    public class ProvidersResponse
    {
        /// <summary>Providers</summary>
        public IEnumerable<string> Providers { get; set; }
    }
}
