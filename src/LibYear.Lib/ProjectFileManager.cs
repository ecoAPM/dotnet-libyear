using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibYear.Lib.FileTypes;

namespace LibYear.Lib
{
    public class ProjectFileManager : IProjectFileManager
    {
        public IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args)
        {
            if (!args.Any())
                return GetProjects(Directory.GetCurrentDirectory());

            var projects = new List<IProjectFile>();
            foreach (var arg in args)
            {
                if (arg.IsCsProjFile())
                    projects.Add(new CsProjFile(arg));
                else if (arg.IsProjectJsonFile())
                    projects.Add(new ProjectJsonFile(arg));
                else if (arg.IsNuGetFile())
                    projects.Add(new PackagesConfigFile(arg));
                else if (Directory.Exists(arg))
                    projects.AddRange(GetProjects(arg));
            }
            return projects;
        }

        public IList<IProjectFile> GetProjects(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var projectFiles = FindProjects(dir, SearchOption.TopDirectoryOnly);
            return projectFiles.Any()
                ? projectFiles
                : FindProjects(dir, SearchOption.AllDirectories);
        }

        public IList<IProjectFile> FindProjects(DirectoryInfo dir, SearchOption searchMode)
        {
            return dir.EnumerateFiles("*.csproj", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName))
                .Union(dir.EnumerateFiles("Directory.build.props", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName)))
                .Union(dir.EnumerateFiles("Directory.build.targets", searchMode).Select<FileInfo, IProjectFile>(f => new CsProjFile(f.FullName)))
                .Union(dir.EnumerateFiles("project.json", searchMode).Select<FileInfo, IProjectFile>(f => new ProjectJsonFile(f.FullName)))
                .Union(dir.EnumerateFiles("packages.config", searchMode).Select<FileInfo, IProjectFile>(f => new PackagesConfigFile(f.FullName)))
                .ToList();
        }

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
}