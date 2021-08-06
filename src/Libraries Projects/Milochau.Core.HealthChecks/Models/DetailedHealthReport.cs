using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace Milochau.Core.HealthChecks.Models
{
    /// <summary>Detailed Health report</summary>
    public class DetailedHealthReport
    {
        /// <summary>Status</summary>
        public DetailedHealthStatus Status { get; set; }

        /// <summary>Total duration</summary>
        public TimeSpan TotalDuration { get; set; }

        /// <summary>Entries</summary>
        public Dictionary<string, DetailedHealthReportEntry> Entries { get; }

        /// <summary>Constructor</summary>
        public DetailedHealthReport(Dictionary<string, DetailedHealthReportEntry> entries, TimeSpan totalDuration)
        {
            Entries = entries;
            TotalDuration = totalDuration;
        }

        /// <summary>Create a detailed health report from a health report</summary>
        public static DetailedHealthReport CreateFrom(HealthReport report)
        {
            var uiReport = new DetailedHealthReport(new Dictionary<string, DetailedHealthReportEntry>(), report.TotalDuration)
            {
                Status = (DetailedHealthStatus)report.Status,
            };

            foreach (var item in report.Entries)
            {
                var entry = new DetailedHealthReportEntry
                {
                    Data = item.Value.Data,
                    Description = item.Value.Description,
                    Duration = item.Value.Duration,
                    Status = (DetailedHealthStatus)item.Value.Status
                };

                if (item.Value.Exception != null)
                {
                    var message = item.Value.Exception?.Message;

                    entry.Exception = message;
                    entry.Description = item.Value.Description ?? message;
                }

                entry.Tags = item.Value.Tags;

                uiReport.Entries.Add(item.Key, entry);
            }

            return uiReport;
        }

        /// <summary>Create a detailed health report from an exception</summary>
        public static DetailedHealthReport CreateFrom(Exception exception, string entryName = "Endpoint")
        {
            var uiReport = new DetailedHealthReport(new Dictionary<string, DetailedHealthReportEntry>(), TimeSpan.FromSeconds(0))
            {
                Status = DetailedHealthStatus.Unhealthy,
            };

            uiReport.Entries.Add(entryName, new DetailedHealthReportEntry
            {
                Exception = exception.Message,
                Description = exception.Message,
                Duration = TimeSpan.FromSeconds(0),
                Status = DetailedHealthStatus.Unhealthy
            });

            return uiReport;
        }
    }
}
