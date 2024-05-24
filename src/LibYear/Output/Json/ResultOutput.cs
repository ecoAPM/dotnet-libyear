using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record ResultOutput
{
	public double YearsBehind { get; init; }
	public double DaysBehind { get; init; }
	public IReadOnlyCollection<ProjectFormatResult> Projects { get; set; } = [];

	public ResultOutput()
	{
	}

	public ResultOutput(SolutionResult solutionResult)
	{
		YearsBehind = solutionResult.YearsBehind;
		DaysBehind = solutionResult.DaysBehind;
		Projects = solutionResult.Details?.Select(project => new ProjectFormatResult(project)).ToArray() ?? Array.Empty<ProjectFormatResult>();
	}
}