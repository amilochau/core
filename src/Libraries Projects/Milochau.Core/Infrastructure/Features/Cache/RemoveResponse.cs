﻿using System.Collections.Generic;

namespace Milochau.Core.Infrastructure.Features.Cache
{
    /// <summary>Response for cache remove endpoint</summary>
    public class RemoveResponse
    {
        /// <summary>Removed keys</summary>
        public ICollection<string> Keys { get; set; }
    }
}
