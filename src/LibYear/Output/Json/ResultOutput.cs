using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record ResultOutput
{
	[JsonConverter(typeof(DoubleFormatter))]
	[JsonPropertyName("yearsBehind")] public double YearsBehind { get; init; }
	[JsonConverter(typeof(DoubleFormatter))]
	[JsonPropertyName("daysBehind")] public double DaysBehind { get; init; }

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