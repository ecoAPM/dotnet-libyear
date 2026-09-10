using NuGet.Protocol.Core.Types;

namespace LibYear.Core;

public class Release(PackageVersion version, DateTime released, bool isPublished = true)
{
	public PackageVersion Version { get; } = version;
	public DateTime Date { get; } = released;
	public bool IsPublished { get; } = isPublished;

	public Release(IPackageSearchMetadata metadata) : this(new PackageVersion(metadata.Identity.Version), metadata.Published.GetValueOrDefault().Date, metadata.IsListed)
	{
	}
}