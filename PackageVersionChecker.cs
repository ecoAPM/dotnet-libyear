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

        private async Task<KeyValuePair<string, IEnumerable<Result>>> GetLibs(string projectFile)
        {
            var deps = XDocument.Load(projectFile).Descendants("PackageReference");

            var results = new List<Result>();
            foreach (var dep in deps)
            {
                try
                {
                    var name = dep.Attribute("Include").Value;
                    var installed = new Version(dep.Attribute("Version").Value);
                    var versionsTask = _versionCache.ContainsKey(name) ? _versionCache[name] : _versionCache[name] = getVersions(name);
                    var versions = await versionsTask;
                    var current = versions.First(v => VersionsMatch(v.Version, installed));
                    var latest = versions.First();
                    results.Add(new Result(name, current, latest));
                }
                catch (Exception)
                {
                    // swallow for now, probably includes prerelease packages
                }
            }
            return new KeyValuePair<string, IEnumerable<Result>>(projectFile, results);
        }

        private async Task<IList<VersionInfo>> getVersions(string name)
        {
            var metadata = _metadataResource.GetMetadataAsync(name, true, true, new NullLogger(), CancellationToken.None);
            return (await metadata).Select(m => new VersionInfo(m)).OrderByDescending(v => v.Version).ToList();
        }

        public static bool VersionsMatch(Version v1, Version v2)
        {
            return v1.Major == v2.Major
                   && (v1.Minor < 0 || v2.Minor < 0 || v1.Minor == v2.Minor)
                   && (v1.Build < 0 || v2.Build < 0 || v1.Build == v2.Build)
                   && (v1.Revision < 0 || v2.Revision < 0 || v1.Revision == v2.Revision);
        }
    }
}