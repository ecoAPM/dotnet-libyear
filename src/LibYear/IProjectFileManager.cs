using System.Collections.Generic;
using LibYear.FileTypes;

namespace LibYear
{
    public interface IProjectFileManager
    {
        IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
        void Update(IProjectFile projectFile, IEnumerable<Result> results);
    }
}