using LibYear.Core.FileTypes;

namespace LibYear.Core.Tests;

public class TestProjectFile : IProjectFile
{
	public string FileName { get; }
	public IDictionary<string, PackageVersion?> Packages { get; }

	public TestProjectFile(string fileName, IDictionary<string, PackageVersion?>? packages = null)
	{
		FileName = fileName;
		Packages = packages ?? new Dictionary<string, PackageVersion?>();
	}

	public string Update(IReadOnlyCollection<Result> results)
		=> string.Empty;
}