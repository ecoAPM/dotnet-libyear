using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibYear.Lib;
using LibYear.Lib.FileTypes;

namespace LibYear.App
{
    public class Runner
    {
        private readonly IPackageVersionChecker _checker;
        private readonly IProjectFileManager _projectFileManager;
        private bool _quietMode;
        private bool _update;

        public Runner(IPackageVersionChecker checker, IProjectFileManager projectFileManager)
        {
            _checker = checker;
            _projectFileManager = projectFileManager;
        }

        public string Run(IReadOnlyList<string> args)
        {
            if (args.Any(a => a == "-h" || a == "--help" || a == "-?" || a == "/?"))
                return GetHelpMessage();
            
            if (args.Any(a => a == "-q" || a == "--quiet"))
                _quietMode = true;

            if (args.Any(a => a == "-u" || a == "--update"))
                _update = true;

            var projects = _projectFileManager.GetAllProjects(args);
            if (!projects.Any())
                return "No project files found";
            
            var msg = new StringBuilder();
            var allResults = _checker.GetPackages(projects);
            var output = GetAllResultsTables(allResults);
            msg.AppendLine(output);

            if (_update)
            {
                var updated = _projectFileManager.Update(allResults);
                foreach (var projectFile in updated)
                    msg.AppendLine($"{projectFile} updated");
            }
            return msg.ToString();
        }

        private static string GetHelpMessage()
        {
            var msg = new StringBuilder();
            msg.AppendLine("Usage: dotnet libyear [args] [{csproj}|{dir}]");
            msg.AppendLine();
            msg.AppendLine("  Zero or more directories or csproj files may be passed");
            msg.AppendLine("  If no arguments are passed, the current directory is searched");
            msg.AppendLine("  If no csproj is found in a directory, subdirectories are searched");
            msg.AppendLine();
            msg.AppendLine("Arguments:");
            msg.AppendLine("  -h, --help \t display this help message");
            msg.AppendLine("  -q, --quiet \t only show outdated packages");
            msg.AppendLine("  -u, --update \t update project files after displaying packages");
            return msg.ToString();
        }

        private string GetAllResultsTables(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
        {
            if (!allResults.Any())
                return string.Empty;

            var namePad = Math.Max("Package".Length, allResults.Max(results => results.Value.Any() ? results.Value.Max(r => r.Name.Length) : 0));
            var installedPad = Math.Max("Installed".Length, allResults.Max(results => results.Value.Any() ? results.Value.Max(r => r.Installed?.Version.ToString().Length ?? 0) : 0));
            var latestPad = Math.Max("Latest".Length, allResults.Max(results => results.Value.Any() ? results.Value.Max(r => r.Latest?.Version.ToString().Length ?? 0) : 0));

            var msg = new StringBuilder();
            foreach (var results in allResults)
                msg.AppendLine(GetResultsTable(results, namePad, installedPad, latestPad));

            if (allResults.Count > 1)
            {
                var allTotal = allResults.Sum(ar => ar.Value.Sum(r => r.YearsBehind));
                msg.AppendLine($"Total is {allTotal:F1} libyears behind");
            }
            return msg.ToString();
        }

        private string GetResultsTable(KeyValuePair<IProjectFile, IEnumerable<Result>> results, int namePad, int installedPad, int latestPad)
        {
            if (!results.Value.Any())
                return string.Empty;

            var msg = new StringBuilder();
            msg.AppendLine(results.Key.FileName);

            msg.AppendLine($"{"Package".PadRight(namePad)}   {"Installed".PadRight(installedPad)}   Released     {"Latest".PadRight(latestPad)}   Released     Age (y)");

            foreach (var result in results.Value.Where(p => !(_quietMode && p.Installed.Version == p.Latest.Version)))
                msg.AppendLine($"{result.Name.PadRight(namePad)}   {result.Installed?.Version.ToString().PadRight(installedPad)}   {result.Installed?.Released:yyyy-MM-dd}   {result.Latest?.Version.ToString().PadRight(latestPad)}   {result.Latest?.Released:yyyy-MM-dd}   {result.YearsBehind:F1}");

            var projectTotal = results.Value.Sum(r => r.YearsBehind);
            msg.AppendLine($"Project is {projectTotal:F1} libyears behind");

            return msg.ToString();
        }
    }
}