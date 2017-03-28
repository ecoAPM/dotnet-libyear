using System.IO;
using System.Linq;
using LibYear.Lib.FileTypes;
using Xunit;

namespace LibYear.Lib.Tests.FileTypes
{
    public class PackagesConfigStreamTests
    {
        [Fact]
        public void CanLoadPackagesConfigStream()
        {
            //arrange
            const string filename = "FileTypes\\packages.config";
            using (var stream = File.OpenRead(filename))
            {
                //act
                var packagesConfigStream = new PackagesConfigStream(stream);

                //assert
                Assert.Equal("test1", packagesConfigStream.Packages.First().Key);
                Assert.Equal("test2", packagesConfigStream.Packages.Skip(1).First().Key);
                Assert.Equal("test3", packagesConfigStream.Packages.Skip(2).First().Key);
            }
        }
    }
}