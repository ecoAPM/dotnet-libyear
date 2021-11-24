using System.IO.Abstractions;
using LibYear.Core.FileTypes;

namespace LibYear.Core;

public class ProjectFileManager : IProjectFileManager
{
	private readonly IFileSystem _fileSystem;

	public ProjectFileManager(IFileSystem fileSystem)
		=> _fileSystem = fileSystem;

	public IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args)
		=> args.Any()
			? args.SelectMany(GetProjects).ToList()
			: GetProjectsInDir(Directory.GetCurrentDirectory());

	private IList<IProjectFile> GetProjects(string arg)
	{
		if (_fileSystem.Directory.Exists(arg))
		{
			return GetProjectsInDir(arg);
		}

		var fileInfo = _fileSystem.FileInfo.FromFileName(arg);
		return new[] { ReadFile(fileInfo) }.Where(f => f != null).ToList()!;
	}

	public IList<IProjectFile> GetProjectsInDir(string dirPath)
	{
		var dir = _fileSystem.DirectoryInfo.FromDirectoryName(dirPath);
		var projectFiles = FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);
		return projectFiles.Any()
			? projectFiles
			: FindProjectsInDir(dir, SearchOption.AllDirectories);
	}

	public IList<IProjectFile> FindProjectsInDir(IDirectoryInfo dir, SearchOption searchMode)
		=> dir.EnumerateFiles("*.csproj", searchMode)
			.Union(dir.EnumerateFiles("Directory.build.props", searchMode))
			.Union(dir.EnumerateFiles("Directory.build.targets", searchMode))
			.Union(dir.EnumerateFiles("packages.config", searchMode))
			.Select<IFileInfo, IProjectFile>(ReadFile!)
			.ToList();

	private static bool IsCsProjFile(IFileSystemInfo fileInfo) => fileInfo.Extension == ".csproj";
	private static bool IsDirectoryBuildPropsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.props";
	private static bool IsDirectoryBuildTargetsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.targets";
	private static bool IsNuGetFile(IFileSystemInfo fileInfo) => fileInfo.Name == "packages.config";

	private IProjectFile? ReadFile(IFileInfo fileInfo)
	{
		var path = fileInfo.FullName;
		var contents = _fileSystem.File.ReadAllText(path);
		if (IsCsProjFile(fileInfo))
			return new CsProjFile(path, contents);
		if (IsDirectoryBuildPropsFile(fileInfo))
			return new DirectoryBuildPropsFile(path, contents);
		if (IsDirectoryBuildTargetsFile(fileInfo))
			return new DirectoryBuildTargetsFile(path, contents);
		if (IsNuGetFile(fileInfo))
			return new PackagesConfigFile(path, contents);

		return null;
	}

	public IEnumerable<string> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
	{
		var updated = new List<string>();
		foreach (var result in allResults)
		{
			var projectFile = result.Key;
			var results = result.Value;
			var update = projectFile.Update(results);
			_fileSystem.File.WriteAllText(projectFile.FileName, update);
			updated.Add(projectFile.FileName);
		}

		return updated;
	}
}