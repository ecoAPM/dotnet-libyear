using Xunit;

namespace LibYear.Lib.Tests
{
    public class PackageVersionTests
    {
        [Fact]
        public void CanCreatePackageVersion()
        {
            var version = new PackageVersion(1,2,3,4);

            Assert.Equal("1.2.3.4", version.ToString());
        }

        [Fact]
        public void CanParseSemVerString()
        {
            var version = PackageVersion.Parse("1.2.3");

            Assert.Equal("1.2.3", version.ToString());
        }

        [Fact]
        public void CanParseNuGetString()
        {
            var version = PackageVersion.Parse("1.2.3.4");

            Assert.Equal("1.2.3.4", version.ToString());
        }

        [Fact]
        public void CanParseWildcardString()
        {
            var version = PackageVersion.Parse("*");

            Assert.Equal("0.0.0", version.ToString());
            Assert.True(version.IsWildcard);
        }
    }
}