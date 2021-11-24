using System.ComponentModel;
using Spectre.Console.Cli;

namespace LibYear;

public class Settings : CommandSettings
{
	[CommandArgument(0, "[target]")]
	[Description("the project file(s) or directory paths to analyze")]
	public string[] Paths { get; set; } = { "." };

	[CommandOption("-u|--update")]
	[Description("update any outdated packages")]
	public bool Update { get; set; }

	[CommandOption("-q|--quiet")]
	[Description("only output outdated packages")]
	public bool QuietMode { get; set; }
}