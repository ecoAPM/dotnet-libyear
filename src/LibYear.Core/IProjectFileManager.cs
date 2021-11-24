using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IProjectFileManager
{
	Task<IList<IProjectFile>> GetAllProjects(IReadOnlyList<string> paths);
	Task<IEnumerable<string>> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults);
}