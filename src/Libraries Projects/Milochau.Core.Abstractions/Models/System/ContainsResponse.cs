using System.Collections.Generic;

namespace Milochau.Core.Abstractions.Models.System
{
    /// <summary>Response for cache contains endpoint</summary>
    public class ContainsResponse
    {
        /// <summary>Tested keys</summary>
        public IEnumerable<string> Keys { get; set; } = new List<string>();

        /// <summary>Contains any of the <see cref="Keys"/></summary>
        public bool Contains { get; set; }
    }
}
