using System.Linq;
using LibYear.FileTypes;
using Xunit;

namespace LibYear.Tests.FileTypes
{
    public class ProjectJsonTests
    {
        [Fact]
        public void CanLoadProjectJsonFile()
        {
            //arrange
            const string filename = "FileTypes\\project.json";

            //act
            var file = new ProjectJsonFile(filename);

            //assert
            Assert.Equal("test1", file.Packages.First().Key);
            Assert.Equal("test2", file.Packages.Skip(1).First().Key);
        }
    }
}
