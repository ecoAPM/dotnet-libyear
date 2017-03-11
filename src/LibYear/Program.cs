using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace LibYear
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Any(a => a == "-h" || a == "--help" || a == "-?" || a == "/?"))
            {
                ShowHelp();
                return;
            }

            var projects = GetAllProjects(args);
            if (!projects.Any())
            {
                Console.WriteLine("No C# projects found");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Running...");
            Console.WriteLine();

            var checker = GetPackageVersionChecker();
            var results = checker.GetPackages(projects);
            DisplayAllResults(results);
        }

        private static PackageVersionChecker GetPackageVersionChecker()
        {
            var metadataResource = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"),
                Repository.Provider.GetCoreV3()).GetResource<PackageMetadataResource>();
            var versionCache = new ConcurrentDictionary<string, IList<VersionInfo>>();
            var checker = new PackageVersionChecker(metadataResource, versionCache);
            return checker;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: dotnet libyear [{csproj}|{dir}] [{csproj}|{dir}]");
            Console.WriteLine();
            Console.WriteLine("Zero or more directories or csproj files may be passed");
            Console.WriteLine("If no arguments are passed, the current directory is searched");
            Console.WriteLine("If no csproj is found in a directory, subdirectories are searched");
            Console.WriteLine();
        }

        private static IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args)
        {
            if (!args.Any())
                return GetProjects(Directory.GetCurrentDirectory());

            var projects = new List<IProjectFile>();
            foreach (var arg in args)
            {
                if (IsProjectFile(arg))
                    projects.Add(new CsProjFile(arg));
                else if (Directory.Exists(arg))
                    projects.AddRange(GetProjects(arg));
            }
            return projects;
        }

        public static bool IsProjectFile(string path) => File.Exists(path) && new FileInfo(path).Extension == "csproj";

        private static IList<IProjectFile> GetProjects(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var csproj = dir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
            return csproj != null
                ? new List<IProjectFile> { new CsProjFile(csproj.FullName) }
                : dir.GetFiles("*.csproj", SearchOption.AllDirectories).Select(f => new CsProjFile(f.FullName) as IProjectFile).ToList();
        }

        private static void DisplayAllResults(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
        {
            foreach (var results in allResults)
                DisplayResults(results);

            if (allResults.Count > 1)
            {
                var allTotal = allResults.Sum(ar => ar.Value.Sum(r => r.YearsBehind));
                Console.WriteLine($"Total is {allTotal:F1} libyears behind");
                Console.WriteLine();
            }
        }

        private static void DisplayResults(KeyValuePair<IProjectFile, IEnumerable<Result>> results)
        {
            Console.WriteLine(results.Key.FileName);

            var namePad = results.Value.Max(r => r.Name.Length);
            var installedPad = results.Value.Max(r => r.Installed.Version.ToString().Length);
            var latestPad = results.Value.Max(r => r.Latest.Version.ToString().Length);
            Console.WriteLine($"{"Package".PadRight(namePad)} \t {"Installed".PadRight(installedPad)} \t Released \t {"Latest".PadRight(latestPad)} \t Released \t Age (y)");

            foreach (var result in results.Value)
                Console.WriteLine($"{result.Name.PadRight(namePad)} \t {result.Installed.Version.ToString().PadRight(installedPad)} \t {result.Installed.Released:yyyy-MM-dd} \t {result.Latest.Version.ToString().PadRight(latestPad)} \t {result.Latest.Released:yyyy-MM-dd} \t {result.YearsBehind:F1}");

            var projectTotal = results.Value.Sum(r => r.YearsBehind);
            Console.WriteLine($"Project is {projectTotal:F1} libyears behind");
            Console.WriteLine();
        }
    }
}