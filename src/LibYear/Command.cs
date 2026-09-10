using Spectre.Console;
using Spectre.Console.Cli;

namespace LibYear;

public class Command(IAnsiConsole console) : AsyncCommand<Settings>
{
	public async Task<int> ExecuteAsync(CommandContext context, Settings settings)
		=> await ExecuteAsync(context, settings, CancellationToken.None);

	protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
	{
		try
		{
			return await console.Status().StartAsync("Running...", Run(settings));
		}
		catch (Exception e)
		{
			console.WriteException(e, ExceptionFormats.ShortenEverything);
			return 1;
		}
	}

	private Func<StatusContext, Task<int>> Run(Settings settings)
		=> async _ => await Factory.App(console).Run(settings);
}