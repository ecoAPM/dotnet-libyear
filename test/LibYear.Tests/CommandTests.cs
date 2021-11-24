using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests;

public class CommandTests
{
	[Fact]
	public async Task CanExecuteCommand()
	{
		//arrange
		var console = new TestConsole();
		var command = new Command(console);

		//act
		var exitCode = await command.ExecuteAsync(null!, new Settings());

		//assert
		Assert.Equal(0, exitCode);
	}
}