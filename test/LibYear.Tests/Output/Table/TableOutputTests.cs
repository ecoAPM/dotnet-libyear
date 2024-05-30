using System.Text.RegularExpressions;
using LibYear.Core;
using LibYear.Core.Tests;
using LibYear.Output.Table;
using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests.Output.Table;

public class TableOutputTests
{
	[Fact]
	public void NoResultsProducesNoOutput()
	{
		// arrange
		var console = new TestConsole();

		// act
		var output = new TableOutput(console);
		var result = new SolutionResult(Array.Empty<ProjectResult>());
		output.DisplayAllResults(result, false);

		// assert
		Assert.Empty(console.Output);
	}

	[Fact]
	public void ShouldPrintATableIfQuietModeDisabled()
	{
		//arrange
		var console = new TestConsole();
		var legacyDateTime = new DateTime(2020, 01, 03);
		var newDateTime = new DateTime(2020, 01, 05);

		// act
		var output = new TableOutput(console);
		var projectFile1 = new TestProjectFile("test project 1");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), legacyDateTime), new Release(new PackageVersion(1, 2, 3), newDateTime)) }),
		});
		output.DisplayAllResults(results, false);

		// assert

		Assert.NotEmpty(console.Output);
		Assert.Contains("│ Package  │ Installed  │ Released    │ Latest  │ Released   │ Age (y) │", console.Output);
		Assert.Contains("│ test1    │ 1.2.3      │ 2020-01-03  │ 1.2.3   │ 2020-01-05 │ 0.0     │", console.Output);
	}

	[Fact]
	public void ShouldPrintSimplifiedIfInQuietMode()
	{
		// arrange
		var console = new TestConsole();

		// act
		var output = new TableOutput(console);
		var projectFile1 = new TestProjectFile("test project 1");
		var results = new SolutionResult(new []
		{
			new ProjectResult(projectFile1, new[] { new Result("test1", new Release(new PackageVersion(1, 2, 3), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)) }),
		});
		output.DisplayAllResults(results, true);

		// assert
		Assert.NotEmpty(console.Output);
		Assert.Contains("  Project is 0.0 libyears ", console.Output);
	}
}