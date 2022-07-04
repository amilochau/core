using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Milochau.Core.Infrastructure.Converters
{
    /// <summary>Converter for <see cref="TimeSpan"/> with System.Text.Json</summary>
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        /// <summary>Read JSON and convert it to <see cref="TimeSpan"/></summary>
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.Parse(reader.GetString() ?? string.Empty);
        }

        /// <summary>Write <see cref="TimeSpan"/> to JSON</summary>
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
