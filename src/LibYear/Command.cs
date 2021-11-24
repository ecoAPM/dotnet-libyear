using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace LibYear;

public class Command : Command<Settings>
{
	private readonly IAnsiConsole _console;

	public Command(IAnsiConsole console)
		=> _console = console;

	public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
	{
		try
		{
			_console.Status().Start("Running...", _ => Factory.App(_console).Run(settings));
			return 0;
		}
		catch (Exception e)
		{
			_console.WriteException(e, ExceptionFormats.ShortenEverything);
			return 1;
		}
	}
}