using System.Collections;
using System.Collections.Generic;
using LibYear.FileTypes;

namespace LibYear
{
    public interface IProjectFileManager
    {
        IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
        IEnumerable<string> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults);
    }
}