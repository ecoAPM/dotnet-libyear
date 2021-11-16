using LibYear.Lib.FileTypes;

namespace LibYear.Lib;

public interface IPackageVersionChecker
{
	IDictionary<IProjectFile, IEnumerable<Result>> GetPackages(IEnumerable<IProjectFile> projectFiles);
}