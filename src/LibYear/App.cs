using LibYear.Core;
using LibYear.Output;
using LibYear.Output.Json;
using LibYear.Output.Table;
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

	public async Task<int> Run(Settings settings)
	{
		_console.WriteLine();
		var projects = await _projectFileManager.GetAllProjects(settings.Paths, settings.Recursive);
		if (projects.Count == 0)
		{
			_console.WriteLine("No project files found");
			return 1;
		}

		var result = await _checker.GetPackages(projects);
		var output = GetOutputMethod(settings);
		output.DisplayAllResults(result, settings.QuietMode);

		if (settings.Update)
		{
			var updated = await _projectFileManager.Update(result);
			foreach (var projectFile in updated)
			{
				_console.WriteLine($"{projectFile} updated");
			}
		}

		var limitChecker = new LimitChecker(settings);
		return limitChecker.AnyLimitsExceeded(result)
			? 1
			: 0;
	}

	private IOutput GetOutputMethod(Settings settings) =>
		settings.Output switch
		{
			OutputOption.Table => new TableOutput(_console),
			OutputOption.Json => new JsonOutput(_console),
			_ => throw new NotImplementedException()
		};
}