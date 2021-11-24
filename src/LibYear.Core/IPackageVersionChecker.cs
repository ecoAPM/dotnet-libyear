using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IPackageVersionChecker
{
	IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles);
}