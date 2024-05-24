using LibYear.Core;
using LibYear.Core.Tests;
using LibYear.Output.Json;
using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests.Output.Json;

public class JsonOutputTests
{
	[Fact]
	public void NoResultsProducesNoOutput()
	{
		//arrange
		var console = new TestConsole
		{
			Profile =
			{
				Width = Int32.MaxValue
			}
		};

		// Act
		var output = new JsonOutput(console);
		var result = new SolutionResult(Array.Empty<ProjectResult>());
		output.DisplayAllResults(result, false);

		// Assert
		Assert.Empty(console.Output);
	}

	[Fact]
	public void QuietModeShouldPrintSingleLine()
	{
		//arrange
		var console = new TestConsole
		{
			Profile =
			{
				Width = Int32.MaxValue // Test console wraps the line, not accurately testing stdout redirection.
			}
		};

		// Act
		var output = new JsonOutput(console);
		var projectFile1 = new TestProjectFile("test project 1");
		var results = new SolutionResult(new[]
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});
		output.DisplayAllResults(results, true);

		// Assert
		Assert.NotEmpty(console.Output);
		Assert.Single(console.Lines);
		Assert.StartsWith("{", console.Output);
		Assert.EndsWith("}", console.Output.TrimEnd());
	}

	[Fact]
	public void NonQuietModeShouldPrintOnMultipleLines()
	{
		//arrange
		var console = new TestConsole();

		// Act
		var output = new JsonOutput(console);
		var projectFile1 = new TestProjectFile("test project 1");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});
		output.DisplayAllResults(results, false);

		// Assert
		Assert.NotEmpty(console.Lines);
		Assert.NotEqual(1, console.Lines.Count);
	}
}