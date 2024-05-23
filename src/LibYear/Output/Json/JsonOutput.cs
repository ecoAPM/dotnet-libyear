using System.Text.Json;
using LibYear.Core;
using Spectre.Console;

namespace LibYear.Output.Json;

internal sealed class JsonOutput : IOutput
{
	private readonly IAnsiConsole _console;

	public JsonOutput(IAnsiConsole console)
	{
		_console = console;
	}

	public void DisplayAllResults(SolutionResult allResults, bool quietMode)
	{
		if (!allResults.Details.Any())
			return;
		var output = FormatOutput(allResults, quietMode);
		_console.WriteLine(output);
	}

	private static string FormatOutput(SolutionResult allResults, bool quietMode)
	{
		var model = new ResultOutput(allResults);
		var serilzationOptions = new JsonSerializerOptions
		{
			Converters =
			{
				new DoubleFormatter(),
				new DateTimeConverter()
			},
			WriteIndented = quietMode
		};
		var output = JsonSerializer.Serialize(model, serilzationOptions);
		return output;
	}
}