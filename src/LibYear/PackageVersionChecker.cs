using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace LibYear
{
    public class PackageVersionChecker
    {
        private readonly PackageMetadataResource _metadataResource;
        private readonly Dictionary<string, Task<IList<VersionInfo>>> _versionCache;

        public PackageVersionChecker(PackageMetadataResource metadataResource)
        {
            _metadataResource = metadataResource;
            _versionCache = new Dictionary<string, Task<IList<VersionInfo>>>();
        }

        public Dictionary<string, IEnumerable<Result>> GetLibs(IEnumerable<string> projectFiles)
        {
            var tasks = projectFiles.Select(GetLibs).ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToDictionary(k => k.Key, v => v.Value);
        }

        public async Task<KeyValuePair<string, IEnumerable<Result>>> GetLibs(string projectFile)
        {
            var deps = XDocument.Load(projectFile).Descendants("PackageReference");

            var results = new List<Result>();
            foreach (var dep in deps)
            {
                try
                {
                    var result = GetResult(dep);
                    results.Add(await result);
                }
                catch (Exception)
                {
                    // swallow for now, probably includes prerelease packages
                }
            }
            return new KeyValuePair<string, IEnumerable<Result>>(projectFile, results);
        }

        public async Task<Result> GetResult(XElement dep)
        {
            var name = dep.Attribute("Include").Value;
            var installed = new Version(dep.Attribute("Version").Value);
            return await GetResult(name, installed);
        }

        public async Task<Result> GetResult(string name, Version installed)
        {
            var versions = await (_versionCache.ContainsKey(name) ? _versionCache[name] : _versionCache[name] = GetVersions(name));
            var current = versions.First(v => v.Matches(installed));
            var latest = versions.First();
            return new Result(name, current, latest);
        }

        public async Task<IList<VersionInfo>> GetVersions(string name)
        {
            var metadata = _metadataResource.GetMetadataAsync(name, true, true, new NullLogger(), CancellationToken.None);
            return (await metadata).Select(m => new VersionInfo(m)).OrderByDescending(v => v.Version).ToList();
        }
    }
}