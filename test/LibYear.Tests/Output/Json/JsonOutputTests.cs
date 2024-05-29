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

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ResultsShouldPrintToConsole(bool quietMode)
	{
		//arrange
		var console = new TestConsole
		{
			Profile =
			{
				Width = Int32.MaxValue
			}
		};
		var projectFile1 = new TestProjectFile("test project 1");
		var solutionResults = new SolutionResult(new[]
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});

		// Act
		var sut = new JsonOutput(console);
		sut.DisplayAllResults(solutionResults, quietMode);

		// Assert
		Assert.NotEmpty(console.Output);
	}

	[Fact]
	public void QuietModeResultInSingleLineOutput()
	{
		//arrange
		var projectFile1 = new TestProjectFile("test project 1");
		var solutionResults = new SolutionResult(new[]
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});

		// Act
		var result = JsonOutput.FormatOutput(solutionResults, true);

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result.Split("\n"));
		Assert.StartsWith("{", result);
		Assert.EndsWith("}", result);
	}

	[Fact]
	public void NonQuietModeShouldResultInMultiLineOutput()
	{
		//arrange

		var projectFile1 = new TestProjectFile("test project 1");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});

		// Act
		var result = JsonOutput.FormatOutput(results, false);

		// Assert
		Assert.NotEmpty(result);
		Assert.NotEqual(1, result.Split("\n").Length);
	}
}