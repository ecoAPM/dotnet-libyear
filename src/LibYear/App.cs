using LibYear.Core;
using LibYear.Core.FileTypes;
using Spectre.Console;

namespace LibYear;

public class App
{
	private readonly IPackageVersionChecker _checker;
	private readonly IProjectFileManager _projectFileManager;
	private readonly IAnsiConsole _console;

	public App(IPackageVersionChecker checker, IProjectFileManager projectFileManager, IAnsiConsole console)
	{
		_checker = checker;
		_projectFileManager = projectFileManager;
		_console = console;
	}

	public async Task Run(Settings settings)
	{
		_console.WriteLine();
		var projects = await _projectFileManager.GetAllProjects(settings.Paths);
		if (!projects.Any())
		{
			_console.WriteLine("No project files found");
			return;
		}

		var allResults = await _checker.GetPackages(projects);
		GetAllResultsTables(allResults, settings.QuietMode);

		if (settings.Update)
		{
			var updated = await _projectFileManager.Update(allResults);
			foreach (var projectFile in updated)
			{
				_console.WriteLine($"{projectFile} updated");
			}
		}
	}

	private void GetAllResultsTables(IDictionary<IProjectFile, IEnumerable<Result>> allResults, bool quietMode)
	{
		if (!allResults.Any())
			return;

		int MaxLength(Func<Result, int> field) => allResults.Max(results => results.Value.Any() ? results.Value.Max(field) : 0);
		var namePad = Math.Max("Package".Length, MaxLength(r => r.Name.Length));
		var installedPad = Math.Max("Installed".Length, MaxLength(r => r.Installed?.Version.ToString().Length ?? 0));
		var latestPad = Math.Max("Latest".Length, MaxLength(r => r.Latest?.Version.ToString().Length ?? 0));

		var width = allResults.Max(r => r.Key.FileName.Length);
		foreach (var results in allResults)
			GetResultsTable(results, width, namePad, installedPad, latestPad, quietMode);

		if (allResults.Count > 1)
		{
			var allTotal = allResults.Sum(ar => ar.Value.Sum(r => r.YearsBehind));
			_console.WriteLine($"Total is {allTotal:F1} libyears behind");
		}
	}

	private void GetResultsTable(KeyValuePair<IProjectFile, IEnumerable<Result>> results, int titlePad, int namePad, int installedPad, int latestPad, bool quietMode)
	{
		if (!results.Value.Any())
			return;

		var projectTotal = results.Value.Sum(r => r.YearsBehind);
		var width = Math.Max(titlePad + 2, namePad + installedPad + latestPad + 48) + 2;
		var table = new Table
		{
			Title = new TableTitle($"  {results.Key.FileName}".PadRight(width)),
			Caption = new TableTitle(($"  Project is {projectTotal:F1} libyears behind").PadRight(width)),
			Width = width
		};
		table.AddColumn(new TableColumn("Package").Width(namePad));
		table.AddColumn(new TableColumn("Installed").Width(installedPad));
		table.AddColumn(new TableColumn("Released"));
		table.AddColumn(new TableColumn("Latest").Width(latestPad));
		table.AddColumn(new TableColumn("Released"));
		table.AddColumn(new TableColumn("Age (y)"));

		foreach (var result in results.Value.Where(r => !quietMode || r.YearsBehind > 0))
		{
			table.AddRow(
				result.Name,
				result.Installed?.Version.ToString() ?? string.Empty,
				result.Installed?.Date.ToString("yyyy-MM-dd") ?? string.Empty,
				result.Latest?.Version.ToString() ?? string.Empty,
				result.Latest?.Date.ToString("yyyy-MM-dd") ?? string.Empty,
				result.YearsBehind.ToString("F1")
			);
		}

		if (quietMode && projectTotal == 0)
		{
			table.ShowHeaders = false;
		}

		_console.Write(table);
		_console.WriteLine();
	}
}