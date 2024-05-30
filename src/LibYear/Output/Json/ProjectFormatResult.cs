using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record ProjectFormatResult
{
	public string Project { get; init; } = string.Empty;
	public double YearsBehind { get; init; }
	public IReadOnlyCollection<PackageResult> Packages { get; init; } = [];

	public ProjectFormatResult(ProjectResult projectResult)
	{
		Project = projectResult.ProjectFile.FileName;
		YearsBehind = projectResult.YearsBehind;
		Packages = projectResult.Details.Select(result => new PackageResult(result)).ToArray();
	}
}