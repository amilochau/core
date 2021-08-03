using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Milochau.Core.Infrastructure.Converters
{
    /// <summary>Health checks response writer</summary>
    /// <remarks>
    /// This code is freely inspired from https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/src/HealthChecks.UI.Client/UIResponseWriter.cs
    /// It has been copied out and modified, in order to avoid dependency to EntityFramework Core
    /// </remarks>
    public static partial class HealthChecksResponseWriter
    {
        /// <summary>JSON options for Health checks serialization</summary>
        /// <remarks>You should use it in a static Lazy, in order to improve performances (JSON serializer options creation is expensive)</remarks>
        public static Lazy<JsonSerializerOptions> JsonOptions = new Lazy<JsonSerializerOptions>(() => CreateJsonOptions());

        private static JsonSerializerOptions CreateJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
                Converters =
                {
                  new JsonStringEnumConverter(),
                  new TimeSpanConverter()
                }
            };
        }
    }
}
