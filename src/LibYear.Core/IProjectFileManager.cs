using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IProjectFileManager
{
	Task<IReadOnlyCollection<IProjectFile>> GetAllProjects(IReadOnlyCollection<string> paths, bool recursive = false);
	Task<IReadOnlyCollection<string>> Update(SolutionResult result);
}