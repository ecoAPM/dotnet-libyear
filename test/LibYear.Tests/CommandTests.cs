using Spectre.Console.Testing;
using Xunit;

namespace LibYear.Tests;

public class CommandTests
{
	[Fact]
	public void CanExecuteCommand()
	{
		//arrange
		var console = new TestConsole();
		var command = new Command(console);

		//act
		var exitCode = command.Execute(null!, new Settings());

		//assert
		Assert.Equal(0, exitCode);
	}
}