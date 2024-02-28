using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace LibYear.Core.FileTypes;

public partial class FileTypePatterns
{

	[GeneratedRegex(".csproj$", RegexOptions.IgnoreCase)]
	public static partial Regex CSProj();

	[GeneratedRegex("^Directory.Build.props$", RegexOptions.IgnoreCase)]
	public static partial Regex DirectoryBuildProps();

	[GeneratedRegex("^Directory.Build.targets$", RegexOptions.IgnoreCase)]
	public static partial Regex DirectoryBuildTargets();

	[GeneratedRegex("^Directory.Packages.props$", RegexOptions.IgnoreCase)]
	public static partial Regex CentralPackageManagement();

	[GeneratedRegex("^packages.config$", RegexOptions.IgnoreCase)]
	public static partial Regex PackagesConfig();
}