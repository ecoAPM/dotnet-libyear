using System.Linq;
using LibYear.FileTypes;
using Xunit;

namespace LibYear.Tests.FileTypes
{
    public class CsProjTests
    {
        [Fact]
        public void CanLoadCsProjFile()
        {
            //arrange
            const string filename = "FileTypes\\project.csproj";

            //act
            var file = new CsProjFile(filename);

            //assert
            Assert.Equal("test1", file.Packages.First().Key);
            Assert.Equal("test2", file.Packages.Skip(1).First().Key);
        }
    }
}