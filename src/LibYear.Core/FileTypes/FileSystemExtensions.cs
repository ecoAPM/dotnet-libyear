using System.IO.Abstractions;

namespace LibYear.Core.FileTypes;

public static class FileSystemExtensions
{
	public static bool IsCsProjFile(this IFileSystemInfo fileInfo)
		=> FileTypePatterns.CSProj().IsMatch(fileInfo.Name);

	public static bool IsDirectoryBuildPropsFile(this IFileSystemInfo fileInfo)
		=> FileTypePatterns.DirectoryBuildProps().IsMatch(fileInfo.Name);

	public static bool IsDirectoryBuildTargetsFile(this IFileSystemInfo fileInfo)
		=> FileTypePatterns.DirectoryBuildTargets().IsMatch(fileInfo.Name);

	public static bool IsCentralPackageManagementFile(this IFileSystemInfo fileInfo)
		=> FileTypePatterns.CentralPackageManagement().IsMatch(fileInfo.Name);

	public static bool IsNuGetFile(this IFileSystemInfo fileInfo)
		=> FileTypePatterns.PackagesConfig().IsMatch(fileInfo.Name);
}