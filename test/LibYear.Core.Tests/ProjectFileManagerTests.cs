using System.IO.Abstractions;
using LibYear.Core.FileTypes;
using Xunit;

namespace LibYear.Core.Tests;

public class ProjectFileManagerTests
{
	[Fact]
	public void CanFindProjectFiles()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);
		var dir = fileSystem.DirectoryInfo.FromDirectoryName("FileTypes");

		//act
		var projects = fileManager.FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void CanFindProjectFilesRecursively()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);
		var dir = fileSystem.DirectoryInfo.FromDirectoryName(".");

		//act
		var projects = fileManager.FindProjectsInDir(dir, SearchOption.AllDirectories);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void CanGetProjectsForDir()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = fileManager.GetProjectsInDir("FileTypes");

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void GetsProjectsRecursivelyIfNoneFound()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = fileManager.GetProjectsInDir(".");

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void CanGetAllProjectsForMultipleArgs()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projectFileNames = new[]
		{
			Path.Combine("FileTypes", "project.csproj"),
			Path.Combine("FileTypes", "packages.config")
		};
		var projects = fileManager.GetAllProjects(projectFileNames);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void GetAllProjectsAddsMultipleProjectsForDirectories()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projectFileNames = new[]
		{
			"FileTypes"
		};
		var projects = fileManager.GetAllProjects(projectFileNames);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void GetAllProjectsGetsCurrentDirectoryByDefault()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = fileManager.GetAllProjects(new List<string>());

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public void CanUpdateProjectFiles()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var allResults = new Dictionary<IProjectFile, IEnumerable<Result>>
		{
			{
				new TestProjectFile("test1"), new[]
				{
					new Result("test1", new Release(new PackageVersion(0, 1, 0), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)),
				}
			}
		};
		var updated = fileManager.Update(allResults);

		//assert
		Assert.Contains("test1", updated);
	}
}