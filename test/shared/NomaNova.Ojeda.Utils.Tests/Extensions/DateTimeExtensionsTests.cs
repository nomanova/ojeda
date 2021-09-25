using System;
using Moq;
using NomaNova.Ojeda.Utils.Extensions;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Utils.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(0, "Right now")]
        [InlineData(3, "A few seconds ago")]
        [InlineData(12, "12 seconds ago")]
        [InlineData(60, "A minute ago")]
        [InlineData(61, "A minute ago")]
        [InlineData(150, "2 minutes ago")]
        [InlineData(59 * 60, "59 minutes ago")]
        [InlineData(60 * 60, "An hour ago")]
        [InlineData(5400, "An hour ago")]
        [InlineData(2 * 60 * 60, "2 hours ago")]
        [InlineData(25 * 60 * 60, "Yesterday")]
        [InlineData(4 * 24 * 60 * 60, "4 days ago")]
        public void ToRelativeTime_WhenValid_ShouldReturnCorrectString(int secondsAgo, string expected)
        {
            // Arrange
            var utcNow = new DateTime(2021, 1, 1);
            var timeKeeperMock = new Mock<ITimeKeeper>();
            timeKeeperMock.Setup(e => e.UtcNow).Returns(utcNow);

            var updatedAt = utcNow.AddSeconds(-secondsAgo);

            // Act
            var actual = updatedAt.ToRelativeTime(timeKeeperMock.Object);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}