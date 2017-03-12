using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace LibYear
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var runner = new Runner(GetPackageVersionChecker());
            Console.WriteLine(runner.Run(args));
        }

        private static IPackageVersionChecker GetPackageVersionChecker()
        {
            var metadataResource = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), Repository.Provider.GetCoreV3()).GetResource<PackageMetadataResource>();
            var versionCache = new ConcurrentDictionary<string, IList<VersionInfo>>();
            return new PackageVersionChecker(metadataResource, versionCache);
        }
    }
}