using System.Collections.Concurrent;
using LibYear.Core.FileTypes;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace LibYear.Core;

public class PackageVersionChecker : IPackageVersionChecker
{
	private readonly PackageMetadataResource _metadataResource;
	private readonly IDictionary<string, IReadOnlyCollection<Release>> _versionCache;

	public PackageVersionChecker(PackageMetadataResource metadataResource)
		: this(metadataResource, new ConcurrentDictionary<string, IReadOnlyCollection<Release>>())
	{
	}

	public PackageVersionChecker(PackageMetadataResource metadataResource, IDictionary<string, IReadOnlyCollection<Release>> versionCache)
	{
		_metadataResource = metadataResource;
		_versionCache = versionCache;
	}

	public async Task<SolutionResult> GetPackages(IReadOnlyCollection<IProjectFile> projectFiles)
	{
		var tasks = projectFiles.ToDictionary(proj => proj, GetResults);
		var results = await Task.WhenAll(tasks.Values);
		return new SolutionResult(results);
	}

	private async Task<ProjectResult> GetResults(IProjectFile proj)
	{
		var tasks = proj.Packages.Select(p => GetResult(p.Key, p.Value));
		var results = await Task.WhenAll(tasks);
		return new ProjectResult(proj, results);
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

	public async Task<IReadOnlyCollection<Release>> GetVersions(string packageName)
	{
		var metadata = await _metadataResource.GetMetadataAsync(packageName, true, true, NullSourceCacheContext.Instance, NullLogger.Instance, CancellationToken.None);
		return metadata.Select(m => new Release(m)).ToArray();
	}
}