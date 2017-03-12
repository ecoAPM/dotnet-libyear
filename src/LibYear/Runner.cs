using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibYear
{
    public class Runner
    {
        private readonly IPackageVersionChecker _checker;
        private bool _quietMode;

        public Runner(IPackageVersionChecker checker)
        {
            _checker = checker;
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

                var projects = GetAllProjects(args);
                if (!projects.Any())
                    msg.AppendLine("No C# projects found");
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
            var csprojs = dir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
            return csprojs != null
                ? new List<IProjectFile> { new CsProjFile(csprojs.FullName) }
                : dir.GetFiles("*.csproj", SearchOption.AllDirectories).Select(f => new CsProjFile(f.FullName) as IProjectFile).ToList();
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