using System.Collections.Generic;
using LibYear.FileTypes;

namespace LibYear
{
    public interface IProjectRetriever
    {
        IList<IProjectFile> GetAllProjects(IReadOnlyList<string> args);
    }
}