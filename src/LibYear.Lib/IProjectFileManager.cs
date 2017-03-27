using System.Collections.Generic;
using LibYear.Lib.FileTypes;

namespace LibYear.Lib
{
    public interface IProjectFileManager
    {
        IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
        IEnumerable<string> Update(IDictionary<IProjectFile, IEnumerable<Result>> allResults);
    }
}