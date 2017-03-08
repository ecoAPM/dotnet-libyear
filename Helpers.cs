using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace LibYear
{
    public static class Helpers
    {
        public static bool IsDirectory(this string path) => Directory.Exists(path);
        public static bool IsProjectFile(this string path) => File.Exists(path) && new FileInfo(path).Extension == "csproj";

        public static async Task ShowDependencies(this string projectFile)
        {
            var results = getResults(projectFile);
            DisplayResults(projectFile, (await results).ToList());
        }

        private static async Task<IEnumerable<Result>> getResults(string projectFile)
        {
            var packageMetadataResource = await GetPackageMetadataResource();
            var deps = XDocument.Load(projectFile).Descendants("PackageReference");

            var results = new List<Result>();
            foreach (var dep in deps)
            {
                try
                {
                    var installed = new Version(dep.Attribute("Version").Value);
                    var name = dep.Attribute("Include").Value;
                    var metadata = await packageMetadataResource.GetMetadataAsync(name, false, true, new NullLogger(),
                        CancellationToken.None);
                    var versions = metadata.Select(m => new VersionInfo(m)).OrderByDescending(v => v.Version);
                    var current = versions.First(v => v.Matches(installed));
                    var latest = versions.First();
                    results.Add(new Result(name, current, latest));
                }
                catch (Exception)
                {
                    // swallow for now, probably includes prerelease packages
                }
            }
            return results;
        }

        private static void DisplayResults(string projectFile, IList<Result> results)
        {
            Console.WriteLine();
            Console.WriteLine(projectFile);

            var pad = results.Max(r => r.Name.Length);
            foreach (var result in results)
                Console.WriteLine($"{result.Name.PadRight(pad)} \t {result.Installed.Version} \t {result.Installed.Released:yyyy-MM-dd} \t {result.Latest.Version} \t {result.Latest.Released:yyyy-MM-dd} \t {result.YearsBehind:F1}");
            var totalYears = results.Sum(r => r.YearsBehind);
            Console.WriteLine($"Project is {totalYears:F1} libyears behind");
            Console.WriteLine();
        }

        private static bool Matches(this VersionInfo v1, Version v2)
        {
            return v1.Version.Major == v2.Major
                && (v1.Version.Minor < 0 || v2.Minor < 0 || v1.Version.Minor == v2.Minor)
                && (v1.Version.Build < 0 || v2.Build < 0 || v1.Version.Build == v2.Build)
                && (v1.Version.Revision < 0 || v2.Revision < 0 || v1.Version.Revision == v2.Revision);
        }

        private static async Task<PackageMetadataResource> GetPackageMetadataResource()
        {
            var nugetRepo = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), Repository.Provider.GetCoreV3());
            return await nugetRepo.GetResourceAsync<PackageMetadataResource>();
        }
    }
}