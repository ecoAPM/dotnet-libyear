using Xunit;

namespace LibYear.Tests
{
    public class FileUtilTests
    {
        [Fact]
        public void RecognizesCsProjFiles()
        {
            Assert.True("FileTypes\\project.csproj".IsCsProjFile());
        }

        [Fact]
        public void RecognizesProjectJsonFiles()
        {
            Assert.True("FileTypes\\project.json".IsProjectJsonFile());
        }

        [Fact]
        public void RecognizesNuGetFiles()
        {
            Assert.True("FileTypes\\packages.config".IsNuGetFile());
        }
    }
}