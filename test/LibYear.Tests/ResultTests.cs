using System;
using Xunit;

namespace LibYear.Tests
{
    public class ResultTests
    {
        [Fact]
        public void YearsBehindCalculatedCorrectly()
        {
            //arrange
            var installed = new VersionInfo(new Version("1.0"), new DateTime(2015, 1, 1));
            var latest = new VersionInfo(new Version("2.0"), new DateTime(2016, 1, 1));

            //act
            var result = new Result("test", installed, latest);
            var yearsBehind = result.YearsBehind;

            //assert
            Assert.Equal(1, yearsBehind);
        }
    }
}