using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace LibYear
{
    public class PackageVersionChecker
    {
        private readonly PackageMetadataResource _metadataResource;
        private readonly IDictionary<string, IList<VersionInfo>> _versionCache;

        public PackageVersionChecker(PackageMetadataResource metadataResource, IDictionary<string, IList<VersionInfo>> versionCache)
        {
            _metadataResource = metadataResource;
            _versionCache = versionCache;
        }

        public IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles)
            => projectFiles.ToDictionary(proj => proj, proj => AwaitResults(proj.Packages.Select(p => GetResultTask(p.Key, p.Value))));

        public static IEnumerable<Result> AwaitResults(IEnumerable<Task<Result>> resultsTasks)
        {
            var tasks = resultsTasks.ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result);
        }

        public async Task<Result> GetResultTask(string packageName, SemanticVersion installed)
        {
            var versions = _versionCache.ContainsKey(packageName) ? _versionCache[packageName] : _versionCache[packageName] = await GetVersions(packageName);
            var current = versions.First(v => v.Version == installed);
            var latest = versions.First(v => v.Version == versions.Max(m => m.Version));
            return new Result(packageName, current, latest);
        }

        public async Task<IList<VersionInfo>> GetVersions(string packageName)
        {
            var metadata = _metadataResource.GetMetadataAsync(packageName, true, true, new NullLogger(), CancellationToken.None);
            return (await metadata).Select(m => new VersionInfo(m)).ToList();
        }
    }
}