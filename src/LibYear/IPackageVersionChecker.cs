using System.Collections.Generic;
using LibYear.FileTypes;

namespace LibYear
{
    public interface IPackageVersionChecker
    {
        IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles);
    }
}