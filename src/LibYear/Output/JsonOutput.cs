using System.Text.Json;
using LibYear.Core;
using Spectre.Console;

namespace LibYear.Output;

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
		var model = new ResultOutput(allResults);
		var serializer = quietMode
			? IndentedJsonSerializerContext.Default.ResultOutput
			: FlatJsonSerializerContext.Default.ResultOutput;
		_console.WriteLine(JsonSerializer.Serialize(model, serializer));
	}
}