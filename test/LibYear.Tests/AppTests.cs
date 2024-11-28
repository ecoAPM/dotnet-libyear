using LibYear.Core;
using LibYear.Core.FileTypes;
using LibYear.Core.Tests;
using NSubstitute;
using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests;

public class AppTests
{
	private static readonly string[] UpdateResult = ["updated"];

	[Fact]
	public async Task UpdateFlagUpdates()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();
		checker.GetPackages(Arg.Any<IReadOnlyCollection<IProjectFile>>()).Returns(new SolutionResult(Array.Empty<ProjectResult>()));

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(new IProjectFile[] { new TestProjectFile("test1") });
		manager.Update(Arg.Any<SolutionResult>()).Returns(UpdateResult);

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings { Update = true });

		//assert
		Assert.Contains("updated", console.Output);
	}

	[Fact]
	public async Task DisplaysPackagesByDefault()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();
		var projectFile = new TestProjectFile("test project");
		var result = new ProjectResult(projectFile, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) });

		var results = new SolutionResult(new[] { result });
		checker.GetPackages(Arg.Any<IReadOnlyCollection<IProjectFile>>()).Returns(results);

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(new IProjectFile[] { projectFile });

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings());

		//assert
		Assert.Contains("test1", console.Output);
	}

	[Fact]
	public async Task QuietModeIgnoresPackageAtNewestVersion()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();
		var projectFile = new TestProjectFile("test project");
		var result = new ProjectResult(projectFile, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) });

		var results = new SolutionResult(new[] { result });
		checker.GetPackages(Arg.Any<IReadOnlyCollection<IProjectFile>>()).Returns(results);

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(new IProjectFile[] { projectFile });

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings { QuietMode = true });

		//assert
		Assert.DoesNotContain("test1", console.Output);
	}

	[Fact]
	public async Task MultiplePackagesShowsGrandTotal()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();
		var projectFile1 = new TestProjectFile("test project 1");
		var projectFile2 = new TestProjectFile("test project 2");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
			new ProjectResult(projectFile2, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) })
		});
		checker.GetPackages(Arg.Any<IReadOnlyCollection<IProjectFile>>()).Returns(results);

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(new IProjectFile[] { projectFile1, projectFile2 });

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings { QuietMode = true });

		//assert
		Assert.Contains("Total", console.Output);
	}

	[Fact]
	public async Task EmptyResultsAreSkipped()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();
		var projectFile1 = new TestProjectFile("test project 1");
		var projectFile2 = new TestProjectFile("test project 2");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
			new ProjectResult(projectFile2, new List<Result>())
		});
		checker.GetPackages(Arg.Any<IReadOnlyCollection<IProjectFile>>()).Returns(results);

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(new IProjectFile[] { projectFile1, projectFile2 });

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings());

		//assert
		Assert.Contains("test project 1", console.Output);
		Assert.DoesNotContain("test project 2", console.Output);
	}

	[Fact]
	public async Task MissingProjectFilesShowsError()
	{
		//arrange
		var checker = Substitute.For<IPackageVersionChecker>();

		var manager = Substitute.For<IProjectFileManager>();
		manager.GetAllProjects(Arg.Any<IReadOnlyCollection<string>>()).Returns(Array.Empty<IProjectFile>());

		var console = new TestConsole();
		var app = new App(checker, manager, console);

		//act
		await app.Run(new Settings());

		//assert
		Assert.Contains("No project files found", console.Output);
	}
}