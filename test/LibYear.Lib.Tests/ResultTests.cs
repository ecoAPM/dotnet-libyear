using System;
using NuGet.Versioning;
using Xunit;

namespace LibYear.Lib.Tests
{
    public class ResultTests
    {
        [Fact]
        public void YearsBehindCalculatedCorrectly()
        {
            //arrange
            var installed = new VersionInfo(new NuGetVersion(1, 0, 0), new DateTime(2015, 1, 1));
            var latest = new VersionInfo(new NuGetVersion(2, 0, 0), new DateTime(2016, 1, 1));

            //act
            var result = new Result("test", installed, latest);
            var yearsBehind = result.YearsBehind;

            //assert
            Assert.Equal(1, yearsBehind);
        }
    }
}