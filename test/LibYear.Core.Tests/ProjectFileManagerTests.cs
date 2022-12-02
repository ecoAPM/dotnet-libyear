using System.IO.Abstractions;
using Xunit;

namespace LibYear.Core.Tests;

public class ProjectFileManagerTests
{
	[Fact]
	public async Task CanFindProjectFiles()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);
		var dir = fileSystem.DirectoryInfo.New("FileTypes");

		//act
		var projects = await fileManager.FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task CanFindProjectFilesRecursively()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);
		var dir = fileSystem.DirectoryInfo.New(".");

		//act
		var projects = await fileManager.FindProjectsInDir(dir, SearchOption.AllDirectories);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task CanGetProjectsForDir()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = await fileManager.GetProjectsInDir("FileTypes");

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task GetsProjectsRecursivelyIfNoneFound()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = await fileManager.GetProjectsInDir(".");

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task CanGetAllProjectsForMultipleArgs()
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
		var projects = await fileManager.GetAllProjects(projectFileNames);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task GetAllProjectsAddsMultipleProjectsForDirectories()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projectFileNames = new[]
		{
			"FileTypes"
		};
		var projects = await fileManager.GetAllProjects(projectFileNames);

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task GetAllProjectsGetsCurrentDirectoryByDefault()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var projects = await fileManager.GetAllProjects(Array.Empty<string>());

		//assert
		Assert.Contains(projects, p => p.FileName.EndsWith("project.csproj"));
		Assert.Contains(projects, p => p.FileName.EndsWith("packages.config"));
	}

	[Fact]
	public async Task CanUpdateProjectFiles()
	{
		//arrange
		var fileSystem = new FileSystem();
		var fileManager = new ProjectFileManager(fileSystem);

		//act
		var allResults = new SolutionResult(new[]
		{
			new ProjectResult(new TestProjectFile("test1"), new[]
			{
				new Result("test1", new Release(new PackageVersion(0, 1, 0), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)),
			})
		});
		var updated = await fileManager.Update(allResults);

		//assert
		Assert.Contains("test1", updated);
	}
}