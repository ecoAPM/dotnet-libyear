using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IPackageVersionChecker
{
	Task<IDictionary<IProjectFile, IEnumerable<Result>>> GetPackages(IEnumerable<IProjectFile> projectFiles);
}