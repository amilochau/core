using System;
using System.Collections.Generic;

namespace Milochau.Core.Infrastructure.Converters
{
    /// <summary>Detailed health report entry</summary>
    public class DetailedHealthReportEntry
    {
        /// <summary>Data</summary>
        public IReadOnlyDictionary<string, object> Data { get; set; }

        /// <summary>Description</summary>
        public string Description { get; set; }

        /// <summary>Duration</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Exception</summary>
        public string Exception { get; set; }

        /// <summary>Status</summary>
        public DetailedHealthStatus Status { get; set; }

        /// <summary>Tags</summary>
        public IEnumerable<string> Tags { get; set; }
    }
}
