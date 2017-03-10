using System;
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

            var projects = getProjects(args);
            if (!projects.Any())
            {
                Console.WriteLine("No C# projects found");
                return;
            }

            var metadataResource = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), Repository.Provider.GetCoreV3()).GetResource<PackageMetadataResource>();
            var checker = new PackageVersionChecker(metadataResource);

            Console.WriteLine();
            Console.WriteLine("Running...");
            Console.WriteLine();

            var results = checker.GetLibs(projects);
            DisplayResults(results);
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

        private static IList<string> getProjects(IReadOnlyList<string> args)
        {
            if (!args.Any())
                return getProjects(Directory.GetCurrentDirectory());

            var projects = new List<string>();
            foreach (var arg in args)
            {
                if (IsProjectFile(arg))
                    projects.Add(arg);
                else if (Directory.Exists(arg))
                    projects.AddRange(getProjects(arg));
            }
            return projects;
        }

        public static bool IsProjectFile(string path) => File.Exists(path) && new FileInfo(path).Extension == "csproj";

        private static IList<string> getProjects(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var csproj = dir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
            return csproj != null
                ? new List<string> { csproj.FullName }
                : dir.GetFiles("*.csproj", SearchOption.AllDirectories).Select(f => f.FullName).ToList();
        }

        private static void DisplayResults(Dictionary<string, IEnumerable<Result>> allResults)
        {
            foreach (var results in allResults)
            {
                Console.WriteLine();
                Console.WriteLine(results.Key);

                var pad = results.Value.Max(r => r.Name.Length);
                Console.WriteLine($"{"Package".PadRight(pad)} \t Installed \t Released \t Latest \t Released \t Age (y)");

                foreach (var result in results.Value)
                    Console.WriteLine($"{result.Name.PadRight(pad)} \t {result.Installed.Version} \t {result.Installed.Released:yyyy-MM-dd} \t {result.Latest.Version} \t {result.Latest.Released:yyyy-MM-dd} \t {result.YearsBehind:F1}");

                var projectTotal = results.Value.Sum(r => r.YearsBehind);
                Console.WriteLine($"Project is {projectTotal:F1} libyears behind");
                Console.WriteLine();
            }
            if (allResults.Count > 1)
            {
                var allTotal = allResults.Sum(ar => ar.Value.Sum(r => r.YearsBehind));
                Console.WriteLine($"Total is {allTotal:F1} libyears behind");
                Console.WriteLine();
            }
        }
    }
}