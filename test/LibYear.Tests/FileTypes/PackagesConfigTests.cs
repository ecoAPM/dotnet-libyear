using System.Linq;
using LibYear.FileTypes;
using Xunit;

namespace LibYear.Tests.FileTypes
{
    public class PackagesConfigTests
    {
        [Fact]
        public void CanLoadPackagesConfigFile()
        {
            //arrange
            const string filename = "FileTypes\\packages.config";

            //act
            var file = new PackagesConfigFile(filename);

            //assert
            Assert.Equal("test1", file.Packages.First().Key);
            Assert.Equal("test2", file.Packages.Skip(1).First().Key);
        }
    }
}