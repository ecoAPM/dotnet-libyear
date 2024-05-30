using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record DisplayVersion
{
	public string VersionNumber { get; init; } = string.Empty;
	public DateTime ReleaseDate { get; init; }
	public DisplayVersion(Release release)
	{
		VersionNumber = release.Version.ToString();
		ReleaseDate = release.Date;
	}
}