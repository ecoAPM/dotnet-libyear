using System.Collections.Concurrent;
using LibYear.Core.FileTypes;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace LibYear.Core;

public class PackageVersionChecker : IPackageVersionChecker
{
	private readonly PackageMetadataResource _metadataResource;
	private readonly IDictionary<string, IList<Release>> _versionCache;

	public PackageVersionChecker(PackageMetadataResource metadataResource)
		: this(metadataResource, new ConcurrentDictionary<string, IList<Release>>())
	{
	}

	public PackageVersionChecker(PackageMetadataResource metadataResource, IDictionary<string, IList<Release>> versionCache)
	{
		_metadataResource = metadataResource;
		_versionCache = versionCache;
	}

	public IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles)
		=> projectFiles.ToDictionary(proj => proj, GetResults);

	private IEnumerable<Result> GetResults(IProjectFile proj)
	{
		var results = proj.Packages.Select(p => GetResult(p.Key, p.Value));
		return Task.WhenAll(results).GetAwaiter().GetResult();
	}

	public async Task<Result> GetResult(string packageName, PackageVersion? installed)
	{
		if (!_versionCache.ContainsKey(packageName))
		{
			_versionCache[packageName] = await GetVersions(packageName);
		}

		var versions = _versionCache[packageName];
		var current = versions.FirstOrDefault(v => v.Version == installed);
		var latest = versions.FirstOrDefault(v => v.Version == versions.Where(m => !m.Version.IsPrerelease && m.IsPublished).Max(m => m.Version));
		if (installed?.IsWildcard ?? false)
		{
			current = latest;
		}

		return new Result(packageName, current, latest);
	}

	public async Task<IList<Release>> GetVersions(string packageName)
	{
		var metadata = await _metadataResource.GetMetadataAsync(packageName, true, true, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);
		return metadata.Select(m => new Release(m)).ToList();
	}
}