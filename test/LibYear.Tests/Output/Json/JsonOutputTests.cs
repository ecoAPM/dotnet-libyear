using LibYear.Core;
using LibYear.Core.Tests;
using LibYear.Output.Json;
using NSubstitute;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests.Output.Json;

public class JsonOutputTests
{
	[Fact]
	public void NoResultsProducesNoOutput()
	{
		//arrange
		var console = Substitute.For<IAnsiConsole>();
		// Act
		var output = new JsonOutput(console);
		var result = new SolutionResult(Array.Empty<ProjectResult>());
		output.DisplayAllResults(result, false);

		// Assert
		console.DidNotReceive().WriteLine();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ResultsShouldPrintToConsole(bool quietMode)
	{
		//arrange
		var console = new TestConsole();
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
		var expectedJsonOutput = @"{""YearsBehind"":0,""DaysBehind"":0,""Projects"":[{""Project"":""test project 1"",""YearsBehind"":0,""Packages"":[{""PackageName"":""test1"",""CurrentVersion"":{""versionNumber"":""1.2.3"",""releaseDate"":""2024-05-29""},""LatestVersion"":{""versionNumber"":""1.2.3"",""releaseDate"":""2024-05-29""},""YearsBehind"":0}]}]}";
		Assert.Equal(expectedJsonOutput, result);
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
		var expectedOutput = """
		                     {
		                       "YearsBehind": 0,
		                       "DaysBehind": 0,
		                       "Projects": [
		                         {
		                           "Project": "test project 1",
		                           "YearsBehind": 0,
		                           "Packages": [
		                             {
		                               "PackageName": "test1",
		                               "CurrentVersion": {
		                                 "versionNumber": "1.2.3",
		                                 "releaseDate": "2024-05-29"
		                               },
		                               "LatestVersion": {
		                                 "versionNumber": "1.2.3",
		                                 "releaseDate": "2024-05-29"
		                               },
		                               "YearsBehind": 0
		                             }
		                           ]
		                         }
		                       ]
		                     }
		                     """;
		Assert.Equal(expectedOutput, result);
	}
}