using LibYear.Core;
using Spectre.Console;

namespace LibYear.Output.Table;

public sealed class TableOutput : IOutput
{
	private readonly IAnsiConsole _console;

	public TableOutput(IAnsiConsole console)
	{
		_console = console;
	}

	public void DisplayAllResults(SolutionResult allResults, bool quietMode)
	{
		if (!allResults.Details.Any())
			return;

		int MaxLength(Func<Result, int> field) => allResults.Details.Max(results => results.Details.Any() ? results.Details.Max(field) : 0);
		var namePad = Math.Max("Package".Length, MaxLength(r => r.Name.Length));
		var installedPad = Math.Max("Installed".Length, MaxLength(r => r.Installed?.Version.ToString().Length ?? 0));
		var latestPad = Math.Max("Latest".Length, MaxLength(r => r.Latest?.Version.ToString().Length ?? 0));
		var width = allResults.Details.Max(r => r.ProjectFile.FileName.Length);
		foreach (var results in allResults.Details)
			GetResultsTable(results, width, namePad, installedPad, latestPad, quietMode);

		if (allResults.Details.Count > 1)
		{
			_console.WriteLine($"Total is {allResults.YearsBehind:F1} libyears behind");
		}
	}

	private void GetResultsTable(ProjectResult results, int titlePad, int namePad, int installedPad, int latestPad, bool quietMode)
	{
		if (results.Details.Count == 0)
			return;

		var width = Math.Max(titlePad + 2, namePad + installedPad + latestPad + 48) + 2;
		var table = new Spectre.Console.Table
		{
			Title = new TableTitle($"  {results.ProjectFile.FileName}".PadRight(width)),
			Caption = new TableTitle(($"  Project is {results.YearsBehind:F1} libyears behind").PadRight(width)),
			Width = width
		};
		table.AddColumn(new TableColumn("Package").Width(namePad));
		table.AddColumn(new TableColumn("Installed").Width(installedPad));
		table.AddColumn(new TableColumn("Released"));
		table.AddColumn(new TableColumn("Latest").Width(latestPad));
		table.AddColumn(new TableColumn("Released"));
		table.AddColumn(new TableColumn("Age (y)"));

		foreach (var result in results.Details.Where(r => !quietMode || r.YearsBehind > 0))
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

		if (quietMode && Math.Abs(results.YearsBehind) < double.Epsilon)
		{
			table.ShowHeaders = false;
		}

		_console.Write(table);
		_console.WriteLine();
	}
}