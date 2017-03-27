using System;
using System.Collections.Generic;
using System.Linq;
using LibYear.Lib.FileTypes;
using Xunit;

namespace LibYear.Lib.Tests.FileTypes
{
    public class CsProjProjectStreamTests
    {
        private static System.IO.Stream LoadStreamFromFile(string fileName)
        {
            return System.IO.File.OpenRead(fileName);
        }

        [Fact]
        public void CanLoadCsProjStream()
        {
            //arrange
            const string filename = "FileTypes\\project.csproj";
            using (var stream = LoadStreamFromFile(filename))
            {
                //act
                var csprojStream = new CsProjStream(stream);

                //assert
                Assert.Equal("test1", csprojStream.Packages.First().Key);
                Assert.Equal("test2", csprojStream.Packages.Skip(1).First().Key);
                Assert.Equal("test3", csprojStream.Packages.Skip(2).First().Key);
            }
        }

        [Fact]
        public void AttemptingToReadFileNameWillThrowNotSupportedException()
        {
            //arrange
            const string filename = "FileTypes\\project.csproj";
            using (var stream = LoadStreamFromFile(filename))
            {
                //act
                var csprojStream = new CsProjStream(stream);

                //assert
                Assert.Throws<NotSupportedException>(() => csprojStream.FileName);
            }
        }

        [Fact]
        public void AttemptingToUpdateWillThrowNotSupportedException()
        {
            //arrange
            const string filename = "FileTypes\\project.csproj";
            using (var stream = LoadStreamFromFile(filename))
            {
                //act
                var csprojStream = new CsProjStream(stream);

                //assert
                Assert.Throws<NotSupportedException>(() => csprojStream.Update(new List<Result>()));
            }
        }
    }
}