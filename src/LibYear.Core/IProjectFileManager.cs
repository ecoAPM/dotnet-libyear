using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IProjectFileManager
{
	Task<IReadOnlyCollection<IProjectFile>> GetAllProjects(IReadOnlyCollection<string> paths);
	Task<IReadOnlyCollection<string>> Update(SolutionResult result);
}