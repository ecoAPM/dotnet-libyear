using System;
using System.IO;
using LibYear.Lib.FileTypes;

namespace LibYear.Lib;

public static class FileUtils
{
	private static bool IsCsProjFile(this string path) => File.Exists(path) && new FileInfo(path).Extension == ".csproj";
	private static bool IsDirectoryBuildPropsFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "Directory.Build.props";
	private static bool IsDirectoryBuildTargetsFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "Directory.Build.targets";
	private static bool IsProjectJsonFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "project.json";
	private static bool IsNuGetFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "packages.config";

	public static IProjectFile ToProjectFile(this string path)
	{
		if (path.IsCsProjFile())
			return new CsProjFile(path);
		if (path.IsDirectoryBuildPropsFile())
			return new DirectoryBuildPropsFile(path);
		if (path.IsDirectoryBuildTargetsFile())
			return new DirectoryBuildTargetsFile(path);
		if (path.IsProjectJsonFile())
			return new ProjectJsonFile(path);
		if (path.IsNuGetFile())
			return new PackagesConfigFile(path);

		return null;
	}
}
