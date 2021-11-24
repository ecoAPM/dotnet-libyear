using LibYear.Core.FileTypes;

namespace LibYear.Core;

public class ProjectFileManager : IProjectFileManager
{
	public IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args)
		=> args.Any()
			? args.SelectMany(GetProjects).ToList()
			: GetProjectsInDir(Directory.GetCurrentDirectory());

	private IList<IProjectFile> GetProjects(string arg)
	{
		return Directory.Exists(arg)
			? GetProjectsInDir(arg)
			: new List<IProjectFile?> { arg.ToProjectFile() }.Where(f => f != null).ToList()!;
	}

	public IList<IProjectFile> GetProjectsInDir(string dirPath)
	{
		var dir = new DirectoryInfo(dirPath);
		var projectFiles = FindProjectsInDir(dir, SearchOption.TopDirectoryOnly);
		return projectFiles.Any()
			? projectFiles
			: FindProjectsInDir(dir, SearchOption.AllDirectories);
	}

	public IList<IProjectFile> FindProjectsInDir(DirectoryInfo dir, SearchOption searchMode)
		=> dir.EnumerateFiles("*.csproj", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName))
			.Union(dir.EnumerateFiles("Directory.build.props", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName)))
			.Union(dir.EnumerateFiles("Directory.build.targets", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName)))
			.Union(dir.EnumerateFiles("project.json", searchMode).Select<FileInfo, IProjectFile>(f => new ProjectJsonFile(f.FullName)))
			.Union(dir.EnumerateFiles("packages.config", searchMode).Select<FileInfo, IProjectFile>(f => new PackagesConfigFile(f.FullName)))
			.ToList();

	public IEnumerable<string> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults)
	{
		var updated = new List<string>();
		foreach (var result in allResults)
		{
			var projectFile = result.Key;
			var results = result.Value;
			projectFile.Update(results);
			updated.Add(projectFile.FileName);
		}
		return updated;
	}
}