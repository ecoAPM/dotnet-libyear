using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record PackageResult
{
	public string PackageName { get; set; } = string.Empty;
	public DisplayVersion? CurrentVersion { get; init; }
	public DisplayVersion? LatestVersion { get; init; }
	public double YearsBehind { get; init; }

	public PackageResult(Result result)
	{
		PackageName = result.Name;
		YearsBehind = result.YearsBehind;
		CurrentVersion = result.Installed is null ? null : new DisplayVersion(result.Installed);
		LatestVersion = result.Latest is null ? null : new DisplayVersion(result.Latest);
	}
}