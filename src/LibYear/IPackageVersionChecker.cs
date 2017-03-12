using System.Collections.Generic;

namespace LibYear
{
    public interface IPackageVersionChecker
    {
        IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles);
    }
}