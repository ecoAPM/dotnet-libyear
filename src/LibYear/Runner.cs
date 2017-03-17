using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibYear.FileTypes;

namespace LibYear
{
    public class Runner
    {
        private readonly IPackageVersionChecker _checker;
        private readonly IProjectRetriever _projectRetriever;
        private bool _quietMode;

        public Runner(IPackageVersionChecker checker, IProjectRetriever projectRetriever)
        {
            _checker = checker;
            _projectRetriever = projectRetriever;
        }

        public string Run(IReadOnlyList<string> args)
        {
            var msg = new StringBuilder();
            if (args.Any(a => a == "-h" || a == "--help" || a == "-?" || a == "/?"))
                msg.AppendLine(GetHelpMessage());
            else
            {
                if (args.Any(a => a == "-q" || a == "--quiet"))
                    _quietMode = true;

                var projects = _projectRetriever.GetAllProjects(args);
                if (!projects.Any())
                    msg.AppendLine("No project files found");
                else
                {
                    msg.AppendLine(GetStartupMessage());
                    var results = _checker.GetPackages(projects);
                    msg.AppendLine(GetAllResultsTables(results));
                }
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
            return msg.ToString();
        }

        private static string GetStartupMessage()
        {
            var msg = new StringBuilder();
            msg.AppendLine("Running...");
            return msg.ToString();
        }

        private string GetAllResultsTables(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
        {
            var msg = new StringBuilder();
            foreach (var results in allResults)
                msg.AppendLine(GetResultsTable(results));

            if (allResults.Count > 1)
            {
                var allTotal = allResults.Sum(ar => ar.Value.Sum(r => r.YearsBehind));
                msg.AppendLine($"Total is {allTotal:F1} libyears behind");
            }
            return msg.ToString();
        }

        private string GetResultsTable(KeyValuePair<IProjectFile, IEnumerable<Result>> results)
        {
            if (!results.Value.Any())
                return string.Empty;

            var msg = new StringBuilder();
            msg.AppendLine(results.Key.FileName);

            var namePad = results.Value.Max(r => r.Name.Length);
            var installedPad = results.Value.Max(r => r.Installed.Version.ToString().Length);
            var latestPad = results.Value.Max(r => r.Latest.Version.ToString().Length);
            msg.AppendLine($"{"Package".PadRight(namePad)} \t {"Installed".PadRight(installedPad)} \t Released \t {"Latest".PadRight(latestPad)} \t Released \t Age (y)");

            foreach (var result in results.Value.Where(p => !(_quietMode && p.Installed.Version == p.Latest.Version)))
                msg.AppendLine($"{result.Name.PadRight(namePad)} \t {result.Installed.Version.ToString().PadRight(installedPad)} \t {result.Installed.Released:yyyy-MM-dd} \t {result.Latest.Version.ToString().PadRight(latestPad)} \t {result.Latest.Released:yyyy-MM-dd} \t {result.YearsBehind:F1}");

            var projectTotal = results.Value.Sum(r => r.YearsBehind);
            msg.AppendLine($"Project is {projectTotal:F1} libyears behind");

            return msg.ToString();
        }
    }
}