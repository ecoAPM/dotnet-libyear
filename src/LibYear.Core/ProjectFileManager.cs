using System.IO.Abstractions;
using LibYear.Core.FileTypes;

namespace LibYear.Core;

public class ProjectFileManager : IProjectFileManager
{
	private readonly IFileSystem _fileSystem;

	public ProjectFileManager(IFileSystem fileSystem)
		=> _fileSystem = fileSystem;

	public async Task<IList<IProjectFile>> GetAllProjects(IReadOnlyList<string> paths)
	{
		if (!paths.Any())
			return await GetProjectsInDir(Directory.GetCurrentDirectory());

		var tasks = paths.Select(GetProjects);
		var projects = await Task.WhenAll(tasks);
		return projects.SelectMany(p => p).ToList();
	}

	private async Task<IList<IProjectFile>> GetProjects(string path)
	{
		if (_fileSystem.Directory.Exists(path))
		{
			return await GetProjectsInDir(path);
		}

		var fileInfo = _fileSystem.FileInfo.FromFileName(path);
		return new[] { await ReadFile(fileInfo) }.Where(f => f != null).ToList();
	}

	public async Task<IList<IProjectFile>> GetProjectsInDir(string dirPath)
	{
		var dir = _fileSystem.DirectoryInfo.FromDirectoryName(dirPath);
		var projectFiles = await FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);
		return projectFiles.Any()
			? projectFiles
			: await FindProjectsInDir(dir, SearchOption.AllDirectories);
	}

	public async Task<IList<IProjectFile>> FindProjectsInDir(IDirectoryInfo dir, SearchOption searchMode)
		=> await Task.WhenAll(FindProjects(dir, searchMode));

	private IEnumerable<Task<IProjectFile>> FindProjects(IDirectoryInfo dir, SearchOption searchMode) =>
		dir.EnumerateFiles("*.csproj", searchMode)
			.Union(dir.EnumerateFiles("Directory.build.props", searchMode))
			.Union(dir.EnumerateFiles("Directory.build.targets", searchMode))
			.Union(dir.EnumerateFiles("packages.config", searchMode))
			.Select(ReadFile)
			.ToList();

	private static bool IsCsProjFile(IFileSystemInfo fileInfo) => fileInfo.Extension == ".csproj";
	private static bool IsDirectoryBuildPropsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.props";
	private static bool IsDirectoryBuildTargetsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.targets";
	private static bool IsNuGetFile(IFileSystemInfo fileInfo) => fileInfo.Name == "packages.config";

	private async Task<IProjectFile> ReadFile(IFileSystemInfo fileInfo)
	{
		var path = fileInfo.FullName;
		var stream = _fileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		var contents = await new StreamReader(stream).ReadToEndAsync();
		if (IsCsProjFile(fileInfo))
			return new CsProjFile(path, contents);
		if (IsDirectoryBuildPropsFile(fileInfo))
			return new DirectoryBuildPropsFile(path, contents);
		if (IsDirectoryBuildTargetsFile(fileInfo))
			return new DirectoryBuildTargetsFile(path, contents);
		if (IsNuGetFile(fileInfo))
			return new PackagesConfigFile(path, contents);

		throw new NotImplementedException("Unknown file type");
	}

	public async Task<IEnumerable<string>> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
	{
		var updated = new List<string>();
		foreach (var result in allResults)
		{
			var projectFile = result.Key;
			var results = result.Value;
			var update = projectFile.Update(results);

			var stream = _fileSystem.FileStream.Create(projectFile.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			await new StreamWriter(stream).WriteAsync(update);
			updated.Add(projectFile.FileName);
		}

		return updated;
	}
}