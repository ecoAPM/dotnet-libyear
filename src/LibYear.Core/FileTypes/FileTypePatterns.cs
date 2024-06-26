﻿using System.Text.RegularExpressions;

namespace LibYear.Core.FileTypes;

public abstract partial class FileTypePatterns
{
	[GeneratedRegex(".csproj$", RegexOptions.IgnoreCase)]
	public static partial Regex CSProj();

	[GeneratedRegex("^Directory.Build.props$", RegexOptions.IgnoreCase)]
	public static partial Regex MSBuildProps();

	[GeneratedRegex("^Directory.Build.targets$", RegexOptions.IgnoreCase)]
	public static partial Regex MSBuildTargets();

	[GeneratedRegex("^Directory.Packages.props$", RegexOptions.IgnoreCase)]
	public static partial Regex CentralPackageManagement();

	[GeneratedRegex("^packages.config$", RegexOptions.IgnoreCase)]
	public static partial Regex PackagesConfig();
}