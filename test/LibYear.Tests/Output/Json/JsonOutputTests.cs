using System.Security.AccessControl;
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

		// act
		var output = new JsonOutput(console);
		var result = new SolutionResult(Array.Empty<ProjectResult>());
		output.DisplayAllResults(result, false);

		// assert
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

		// act
		var sut = new JsonOutput(console);
		sut.DisplayAllResults(solutionResults, quietMode);

		// assert
		Assert.NotEmpty(console.Output);
	}

	[Fact]
	public void QuietModeResultInSingleLineOutput()
	{
		//arrange
		var projectFile1 = new TestProjectFile("test project 1");
		var dateTime = new DateTime(2020, 01, 02);
		var solutionResults = new SolutionResult(new[]
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), dateTime), new Release(new PackageVersion(1, 2, 3), dateTime)) }),
		});

		// act
		var result = JsonOutput.FormatOutput(solutionResults, true);

		// assert
		var expectedJsonOutput = @"{""YearsBehind"":0,""DaysBehind"":0,""Projects"":[{""Project"":""test project 1"",""YearsBehind"":0,""Packages"":[{""PackageName"":""test1"",""CurrentVersion"":{""VersionNumber"":""1.2.3"",""ReleaseDate"":""2020-01-02""},""LatestVersion"":{""VersionNumber"":""1.2.3"",""ReleaseDate"":""2020-01-02""},""YearsBehind"":0}]}]}";
		Assert.Equal(expectedJsonOutput, result);
	}

	[Fact]
	public void NonQuietModeShouldResultInMultiLineOutput()
	{
		//arrange

		var projectFile1 = new TestProjectFile("test project 1");
		var dateTime = new DateTime(2020, 01, 02);
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), dateTime), new Release(new PackageVersion(1, 2, 3), dateTime)) }),
		});

		// act
		var result = JsonOutput.FormatOutput(results, false);

		// assert
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
		                                 "VersionNumber": "1.2.3",
		                                 "ReleaseDate": "2020-01-02"
		                               },
		                               "LatestVersion": {
		                                 "VersionNumber": "1.2.3",
		                                 "ReleaseDate": "2020-01-02"
		                               },
		                               "YearsBehind": 0
		                             }
		                           ]
		                         }
		                       ]
		                     }
		                     """;
		Assert.Equal(expectedOutput.ReplaceLineEndings(), result);
	}
}