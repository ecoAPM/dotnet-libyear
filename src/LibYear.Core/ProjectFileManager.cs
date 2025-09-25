using LibYear.Core.FileTypes;
using System.IO.Abstractions;

namespace LibYear.Core;

public class ProjectFileManager : IProjectFileManager
{
	private readonly IFileSystem _fileSystem;

	public ProjectFileManager(IFileSystem fileSystem)
		=> _fileSystem = fileSystem;

	public async Task<IReadOnlyCollection<IProjectFile>> GetAllProjects(IReadOnlyCollection<string> paths, bool recursive = false)
	{
		if (paths.Count == 0)
			return await GetProjectsInDir(Directory.GetCurrentDirectory(), recursive);

		var tasks = paths.Select(p => GetProjects(p, recursive));
		var projects = await Task.WhenAll(tasks);
		return projects.SelectMany(p => p).ToArray();
	}

	private async Task<IReadOnlyCollection<IProjectFile>> GetProjects(string path, bool recursive)
	{
		if (_fileSystem.Directory.Exists(path))
		{
			return await GetProjectsInDir(path, recursive);
		}

		var fileInfo = _fileSystem.FileInfo.New(path);
		return [await ReadFile(fileInfo)];
	}

	public async Task<IReadOnlyCollection<IProjectFile>> GetProjectsInDir(string dirPath, bool recursive)
	{
		var dir = _fileSystem.DirectoryInfo.New(dirPath);
		var projectFiles = await FindProjectsInDir(dir, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		return projectFiles.Count > 0
			? projectFiles
			: await FindProjectsInDir(dir, SearchOption.AllDirectories);
	}

	public async Task<IReadOnlyCollection<IProjectFile>> FindProjectsInDir(IDirectoryInfo dir, SearchOption searchMode)
		=> await Task.WhenAll(FindProjects(dir, searchMode));

	private Task<IProjectFile>[] FindProjects(IDirectoryInfo dir, SearchOption searchMode) =>
		dir.EnumerateFiles("*.*", searchMode)
			.Where(f => f.IsCsProjFile()
				|| f.IsNuGetFile()
				|| f.IsMSBuildPropsFile()
				|| f.IsMSBuildTargetsFile()
				|| f.IsCentralPackageManagementFile())
			.Select(ReadFile)
			.ToArray();

	private async Task<IProjectFile> ReadFile(IFileSystemInfo fileInfo)
	{
		var path = fileInfo.FullName;
		var stream = _fileSystem.FileStream.New(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		var contents = await new StreamReader(stream).ReadToEndAsync();
		stream.Close();

		if (fileInfo.IsCsProjFile())
			return new CsProjFile(path, contents);
		if (fileInfo.IsMSBuildPropsFile())
			return new MSBuildPropsFile(path, contents);
		if (fileInfo.IsMSBuildTargetsFile())
			return new MSBuildTargetsFile(path, contents);
		if (fileInfo.IsNuGetFile())
			return new PackagesConfigFile(path, contents);
		if (fileInfo.IsCentralPackageManagementFile())
			return new CentralPackageManagementFile(path, contents);

		throw new NotImplementedException("Unknown file type");
	}

	public async Task<IReadOnlyCollection<string>> Update(SolutionResult result)
	{
		var updated = new List<string>();
		foreach (var project in result.Details)
		{
			var update = project.ProjectFile.Update(project.Details);

			await _fileSystem.File.WriteAllTextAsync(project.ProjectFile.FileName, update);
			updated.Add(project.ProjectFile.FileName);
		}

		return updated;
	}
}