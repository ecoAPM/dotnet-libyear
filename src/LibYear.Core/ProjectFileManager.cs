using LibYear.Core.FileTypes;
using System.IO.Abstractions;

namespace LibYear.Core;

public class ProjectFileManager : IProjectFileManager
{
	private readonly IFileSystem _fileSystem;

	public ProjectFileManager(IFileSystem fileSystem)
		=> _fileSystem = fileSystem;

	public async Task<IReadOnlyCollection<IProjectFile>> GetAllProjects(IReadOnlyCollection<string> paths)
	{
		if (!paths.Any())
			return await GetProjectsInDir(Directory.GetCurrentDirectory());

		var tasks = paths.Select(GetProjects);
		var projects = await Task.WhenAll(tasks);
		return projects.SelectMany(p => p).ToArray();
	}

	private async Task<IReadOnlyCollection<IProjectFile>> GetProjects(string path)
	{
		if (_fileSystem.Directory.Exists(path))
		{
			return await GetProjectsInDir(path);
		}

		var fileInfo = _fileSystem.FileInfo.New(path);
		return new[] { await ReadFile(fileInfo) }.ToArray();
	}

	public async Task<IReadOnlyCollection<IProjectFile>> GetProjectsInDir(string dirPath)
	{
		var dir = _fileSystem.DirectoryInfo.New(dirPath);
		var projectFiles = await FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);
		return projectFiles.Any()
			? projectFiles
			: await FindProjectsInDir(dir, SearchOption.AllDirectories);
	}

	public async Task<IReadOnlyCollection<IProjectFile>> FindProjectsInDir(IDirectoryInfo dir, SearchOption searchMode)
		=> await Task.WhenAll(FindProjects(dir, searchMode));

	private IReadOnlyCollection<Task<IProjectFile>> FindProjects(IDirectoryInfo dir, SearchOption searchMode) =>
		dir.EnumerateFiles("*.csproj", searchMode)
			.Union(dir.EnumerateFiles("Directory.build.props", searchMode))
			.Union(dir.EnumerateFiles("Directory.build.targets", searchMode))
			.Union(dir.EnumerateFiles("packages.config", searchMode))
			.Union(dir.EnumerateFiles("Directory.packages.props", searchMode))
			.Select(ReadFile)
			.ToArray();

	private static bool IsCsProjFile(IFileSystemInfo fileInfo) => fileInfo.Extension == ".csproj";
	private static bool IsDirectoryBuildPropsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.props";
	private static bool IsDirectoryBuildTargetsFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Build.targets";
	private static bool IsNuGetFile(IFileSystemInfo fileInfo) => fileInfo.Name == "packages.config";
	private static bool IsCentralPackageManagementFile(IFileSystemInfo fileInfo) => fileInfo.Name == "Directory.Packages.props";

	private async Task<IProjectFile> ReadFile(IFileSystemInfo fileInfo)
	{
		var path = fileInfo.FullName;
		var stream = _fileSystem.FileStream.New(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		var contents = await new StreamReader(stream).ReadToEndAsync();
		stream.Close();

		if (IsCsProjFile(fileInfo))
			return new CsProjFile(path, contents);
		if (IsDirectoryBuildPropsFile(fileInfo))
			return new DirectoryBuildPropsFile(path, contents);
		if (IsDirectoryBuildTargetsFile(fileInfo))
			return new DirectoryBuildTargetsFile(path, contents);
		if (IsNuGetFile(fileInfo))
			return new PackagesConfigFile(path, contents);
		if (IsCentralPackageManagementFile(fileInfo))
			return new CentralPackageManagementFile(path, contents);

		throw new NotImplementedException("Unknown file type");
	}

	public async Task<IReadOnlyCollection<string>> Update(SolutionResult result)
	{
		var updated = new List<string>();
		foreach (var project in result.Details)
		{
			var update = project.ProjectFile.Update(project.Details);

			var stream = _fileSystem.FileStream.New(project.ProjectFile.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			await new StreamWriter(stream).WriteAsync(update);
			stream.Close();
			updated.Add(project.ProjectFile.FileName);
		}

		return updated;
	}
}