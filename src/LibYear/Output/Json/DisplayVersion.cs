using System.Text.Json.Serialization;
using LibYear.Core;

namespace LibYear.Output.Json;

internal sealed record DisplayVersion
{
	[JsonPropertyName("versionNumber")]
	public string VersionNumber { get; init; } = string.Empty;
	[JsonConverter(typeof(DateTimeConverter))]
	[JsonPropertyName("releaseDate")]
	public DateTime ReleaseDate { get; init; }
	public DisplayVersion(Release release)
	{
		VersionNumber = release.Version.ToString();
		ReleaseDate = release.Date;
	}
}