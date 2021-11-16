using NuGet.Protocol.Core.Types;

namespace LibYear.Lib;

public class Release
{
	public PackageVersion Version { get; }
	public DateTime Date { get; }
	public bool IsPublished { get; }

	public Release(IPackageSearchMetadata metadata) : this(new PackageVersion(metadata.Identity.Version), metadata.Published.GetValueOrDefault().Date, metadata.IsListed)
	{
	}

	public Release(PackageVersion version, DateTime released, bool isPublished = true)
	{
		Version = version;
		Date = released;
		IsPublished = isPublished;
	}
}