using LibYear.Lib.FileTypes;
using Xunit;

namespace LibYear.Lib.Tests
{
    public class FileUtilTests
    {
        [Fact]
        public void RecognizesCsProjFiles()
        {
            Assert.IsType<CsProjFile>("FileTypes\\project.csproj".ToProjectFile());
        }

        [Fact]
        public void RecognizesDirectoryBuildPropsFiles()
        {
            Assert.IsType<DirectoryBuildPropsFile>("FileTypes\\Directory.Build.props".ToProjectFile());
        }

        [Fact]
        public void RecognizesDirectoryBuildTargetsFiles()
        {
            Assert.IsType<DirectoryBuildTargetsFile>("FileTypes\\Directory.Build.targets".ToProjectFile());
        }

        [Fact]
        public void RecognizesProjectJsonFiles()
        {
            Assert.IsType<ProjectJsonFile>("FileTypes\\project.json".ToProjectFile());
        }

        [Fact]
        public void RecognizesNuGetFiles()
        {
            Assert.IsType<PackagesConfigFile>("FileTypes\\packages.config".ToProjectFile());
        }

        [Fact]
        public void UnrecognizedFilesReturnNull()
        {
            Assert.Null("xunit.runner.json".ToProjectFile());
        }
    }
}