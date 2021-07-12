using Milochau.Core.Infrastructure.Features.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Milochau.Core.Tests.Infrastructure.Features.Cache
{
    [TestClass]
    public class ApplicationMemoryCacheTests
    {
        private ApplicationMemoryCache cache;

        [TestInitialize]
        public void Initialize()
        {
            var memoryCacheOptions = new MemoryCacheOptions();
            var memoryCacheOptionsOptions = new Mock<IOptions<MemoryCacheOptions>>();
            memoryCacheOptionsOptions.SetupGet(x => x.Value).Returns(memoryCacheOptions);

            cache = new ApplicationMemoryCache(memoryCacheOptionsOptions.Object);
        }

        [TestMethod("GetOrCreate - Two calls")]
        public void GetOrCreate_Should_CreateFirstGetSecond_When_CalledTwice()
        {
            // Arrange
            var key = "testKey";
            var valueFactory = new Mock<Func<string>>();
            var duration = TimeSpan.FromMinutes(10);

            // Act & Assert
            cache.GetOrCreate(key, valueFactory.Object, duration);
            valueFactory.Verify(x => x.Invoke(), Times.Once);
            cache.GetOrCreate(key, valueFactory.Object, duration);
            valueFactory.Verify(x => x.Invoke(), Times.Once);
        }

        [TestMethod("GetOrCreateAsync - Two calls")]
        public async Task GetOrCreateAsync_Should_CreateFirstGetSecond_When_CalledTwiceAsync()
        {
            // Arrange
            var key = "testKey";
            var valueFactory = new Mock<Func<Task<string>>>();
            var duration = TimeSpan.FromMinutes(10);

            // Act & Assert
            await cache.GetOrCreateAsync(key, valueFactory.Object, duration);
            valueFactory.Verify(x => x.Invoke(), Times.Once);
            await cache.GetOrCreateAsync(key, valueFactory.Object, duration);
            valueFactory.Verify(x => x.Invoke(), Times.Once);
        }

        [TestMethod("Contains - Two calls")]
        [DataRow("testKey", true, DisplayName = "Key has been set before")]
        [DataRow("newKey", false, DisplayName = "Key has never been set")]
        public void Contains_Should_ReturnExistance_When_Called(string keyToTest, bool expected)
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var duration = TimeSpan.FromMinutes(10);
            cache.GetOrCreate(key, () => value, duration);

            // Act
            var exists = cache.Contains(keyToTest);

            // Assert
            Assert.AreEqual(expected, exists);
        }

        [TestMethod("Set, Remove - Called")]
        public void SetRemove_Should_SetThenRemoveItem_When_Called()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var duration = TimeSpan.FromMinutes(10);

            // Act & Assert - Set
            var item = cache.Set(key, value, duration);

            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(value, item);

            // Act & Assert - Remove
            cache.Remove(key);

            Assert.AreEqual(0, cache.Count);
        }
    }
}
