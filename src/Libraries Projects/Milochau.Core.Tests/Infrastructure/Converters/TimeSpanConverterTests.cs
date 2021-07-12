using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milochau.Core.Infrastructure.Converters;
using System;
using System.Text.Json;

namespace Milochau.Core.Tests.Infrastructure.Converters
{
    [TestClass]
    public class TimeSpanConverterTests
    {
        [TestMethod("Read - Real TimeSpan")]
        public void Read_When_RealTimeSpan()
        {
            // Given
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeSpanConverter());

            // When
            var result = JsonSerializer.Deserialize<TimeSpan>("\"01:30:10.0020000\"", options);

            // Then
            Assert.AreEqual(new TimeSpan(0, 1, 30, 10, 2), result);
        }

        [TestMethod("Write - Read TimeSpan")]
        public void Write_When_RealTimeSpan()
        {
            // Given
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeSpanConverter());

            // When
            var result = JsonSerializer.Serialize(new TimeSpan(0, 1, 30, 10, 2), options);

            // Then
            Assert.AreEqual("\"01:30:10.0020000\"", result);
        }
    }
}
