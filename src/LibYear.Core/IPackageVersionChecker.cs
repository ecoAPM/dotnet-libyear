using LibYear.Core.FileTypes;

namespace LibYear.Core;

public interface IPackageVersionChecker
{
	Task<SolutionResult> GetPackages(IReadOnlyCollection<IProjectFile> projectFiles);
}