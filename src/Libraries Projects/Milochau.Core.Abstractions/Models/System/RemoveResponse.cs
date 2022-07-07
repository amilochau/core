using System.Collections.Generic;

namespace Milochau.Core.Abstractions.Models.System
{
    /// <summary>Response for cache remove endpoint</summary>
    public class RemoveResponse
    {
        /// <summary>Removed keys</summary>
        public ICollection<string> Keys { get; set; } = new List<string>();
    }
}
