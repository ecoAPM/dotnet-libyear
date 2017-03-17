using System.Collections.Generic;
using LibYear.FileTypes;

namespace LibYear
{
    public interface IProjectFileManager
    {
        IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
        void UpdateAll(IDictionary<IProjectFile, IEnumerable<Result>> allResults);
    }
}