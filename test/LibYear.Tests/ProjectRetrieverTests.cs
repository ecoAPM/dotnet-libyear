using System.IO;
using System.Linq;
using Xunit;

namespace LibYear.Tests
{
    public class ProjectRetrieverTests
    {
        [Fact]
        public void CanFindProjectFiles()
        {
            //arrange
            var retriever = new ProjectRetriever();
            var dir = new DirectoryInfo("FileTypes");

            //act
            var projects = retriever.FindProjects(dir, SearchOption.TopDirectoryOnly);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void CanFindProjectFilesRecursively()
        {
            //arrange
            var retriever = new ProjectRetriever();
            var dir = new DirectoryInfo(".");

            //act
            var projects = retriever.FindProjects(dir, SearchOption.AllDirectories);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void CanGetProjectsForDir()
        {
            //arrange
            var retriever = new ProjectRetriever();

            //act
            var projects = retriever.GetProjects("FileTypes");

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void GetsProjectsRecursivelyIfNoneFound()
        {
            //arrange
            var retriever = new ProjectRetriever();

            //act
            var projects = retriever.GetProjects(".");

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void CanGetAllProjectsForMultipleArgs()
        {
            //arrange
            var projectFileNames = new[]
            {
                "FileTypes\\project.csproj",
                "FileTypes\\project.json",
                "FileTypes\\packages.config",
            };
            var retriever = new ProjectRetriever();

            //act
            var projects = retriever.GetAllProjects(projectFileNames);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }
    }
}