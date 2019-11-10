using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LibYear.Lib;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using VersionInfo = LibYear.Lib.VersionInfo;

namespace LibYear.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var metadataResource = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), Repository.Provider.GetCoreV3()).GetResource<PackageMetadataResource>();
                var versionCache = new ConcurrentDictionary<string, IList<VersionInfo>>();
                var packageVersionChecker = new PackageVersionChecker(metadataResource, versionCache);
                var projectRetriever = new ProjectFileManager();

                var runner = new Runner(packageVersionChecker, projectRetriever);
                var output = runner.Run(args);
                Console.WriteLine(output);
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry, an unexpected exception has occurred: " + e);
            }
        }
    }
}