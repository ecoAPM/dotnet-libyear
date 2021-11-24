using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IProjectFileManager
{
	IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
	IEnumerable<string> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults);
}