using LibYear.Core.FileTypes;

namespace LibYear.Core.Tests;

public class TestProjectFile(string fileName, IDictionary<string, PackageVersion?>? packages = null) : IProjectFile
{
	public string FileName { get; } = fileName;
	public IDictionary<string, PackageVersion?> Packages { get; } = packages ?? new Dictionary<string, PackageVersion?>();

	public string Update(IReadOnlyCollection<Result> results)
		=> string.Empty;
}