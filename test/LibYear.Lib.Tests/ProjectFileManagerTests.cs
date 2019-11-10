using System;
using System.Collections.Generic;
using System.IO;
using LibYear.Lib.FileTypes;
using NuGet.Versioning;
using Xunit;

namespace LibYear.Lib.Tests
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
            var projects = fileManager.FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void CanFindProjectFilesRecursively()
        {
            //arrange
            var fileManager = new ProjectFileManager();
            var dir = new DirectoryInfo(".");

            //act
            var projects = fileManager.FindProjectsInDir(dir, SearchOption.AllDirectories);

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void CanGetProjectsForDir()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetProjectsInDir("FileTypes");

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void GetsProjectsRecursivelyIfNoneFound()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetProjectsInDir(".");

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void CanGetAllProjectsForMultipleArgs()
        {
            //arrange
            var projectFileNames = new[]
            {
                Path.Combine("FileTypes", "project.csproj"),
                Path.Combine("FileTypes", "project.json"),
                Path.Combine("FileTypes", "packages.config")
            };
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetAllProjects(projectFileNames);

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
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
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void GetAllProjectsGetsCurrentDirectoryByDefault()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act
            var projects = fileManager.GetAllProjects(new List<string>());

            //assert
            Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
            Assert.Contains(projects, p => p.FileName.EndsWith("project.json"));
            Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
        }

        [Fact]
        public void CanUpdateProjectFiles()
        {
            //arrange
            var fileManager = new ProjectFileManager();

            //act

            var allResults = new Dictionary<IProjectFile, IEnumerable<Result>>
            {
                {
                    new TestProjectFile("test1"), new List<Result>
                    {
                        new Result("test1", new VersionInfo(new SemanticVersion(0, 1, 0), DateTime.Today), new VersionInfo(new SemanticVersion(1, 2, 3), DateTime.Today)),
                    }
                }
            };
            var updated = fileManager.Update(allResults);

            //assert
            Assert.Contains("test1", updated);
        }
    }
}