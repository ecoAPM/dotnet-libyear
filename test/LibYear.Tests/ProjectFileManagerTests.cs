using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace LibYear.Tests
{
    public class ProjectFileManagerTests
    {
        [Fact]
        public void CanFindProjectFiles()
        {
            //arrange
            var fileManager = new ProjectFileManager();
            var dir = new DirectoryInfo("FileTypes");

            //act
            var projects = fileManager.FindProjects(dir, SearchOption.TopDirectoryOnly);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void CanFindProjectFilesRecursively()
        {
            //arrange
            var fileManager = new ProjectFileManager();
            var dir = new DirectoryInfo(".");

            //act
            var projects = fileManager.FindProjects(dir, SearchOption.AllDirectories);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void CanGetProjectsForDir()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetProjects("FileTypes");

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void GetsProjectsRecursivelyIfNoneFound()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetProjects(".");

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
                "FileTypes\\packages.config"
            };
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetAllProjects(projectFileNames);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void GetAllProjectsAddsMultipleProjectsForDirectories()
        {
            //arrange
            var projectFileNames = new[]
            {
                "FileTypes"
            };
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetAllProjects(projectFileNames);

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }

        [Fact]
        public void GetAllProjectsGetsCurrentDirectoryByDefault()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetAllProjects(new List<string>());

            //assert
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.csproj")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("project.json")));
            Assert.True(projects.Any(p => p.FileName.EndsWith("packages.config")));
        }
    }
}