using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output;

internal sealed record ProjectFormatResult
{
	[JsonPropertyName("project")]
	public string Project { get; init; } = string.Empty;
	[JsonConverter(typeof(DoubleFormatter))]
	[JsonPropertyName("daysBehind")]
	public double DaysBehind { get; init; }
	[JsonConverter(typeof(DoubleFormatter))]
	[JsonPropertyName("yearsBehind")]
	public double YearsBehind { get; init; }
	[JsonPropertyName("packages")]
	public IReadOnlyCollection<PackageResult> Packages { get; init; } = [];

	public ProjectFormatResult(ProjectResult projectResult)
	{
		Project = projectResult.ProjectFile.FileName;
		DaysBehind = projectResult.DaysBehind;
		YearsBehind = projectResult.YearsBehind;
		Packages = projectResult.Details.Select(result => new PackageResult(result)).ToArray();
	}
}