using System;
using Xunit;

namespace LibYear.Tests
{
    public class VersionInfoTests
    {
        [Theory]
        [InlineData("1.0.0.0", "1.0.0.0")]
        [InlineData("1.0.0.0", "1.0.0")]
        [InlineData("1.0.0.0", "1.0")]
        [InlineData("1.0.0", "1.0.0.0")]
        [InlineData("1.0.0", "1.0.0")]
        [InlineData("1.0.0", "1.0")]
        [InlineData("1.0", "1.0.0.0")]
        [InlineData("1.0", "1.0.0")]
        [InlineData("1.0", "1.0")]
        public void VersionsMatchWhenEqual(string installed, string latest)
        {
            //arrange
            var versionInfo = new VersionInfo(new Version(installed), new DateTime());
            var other = new Version(latest);

            //act
            var equal = versionInfo.Matches(other);

            //assert
            Assert.True(equal);
        }
    }
}