using System.Collections.Generic;
using LibYear.Lib.FileTypes;

namespace LibYear.Lib.Tests;

public class TestProjectFile : IProjectFile
{
	public string FileName { get; }
	public IDictionary<string, PackageVersion> Packages { get; }

	public TestProjectFile(string fileName, IDictionary<string, PackageVersion> packages = null)
	{
		FileName = fileName;
		Packages = packages;
	}

	public void Update(IEnumerable<Result> results)
	{
	}
}