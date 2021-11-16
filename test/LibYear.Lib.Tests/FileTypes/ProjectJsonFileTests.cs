using LibYear.Lib.FileTypes;
using Xunit;

namespace LibYear.Lib.Tests.FileTypes;

public class ProjectJsonFileTests
{
	[Fact]
	public void CanLoadProjectJsonFile()
	{
		//arrange
		var filename = Path.Combine("FileTypes", "project.json");

		//act
		var file = new ProjectJsonFile(filename);

		//assert
		Assert.Equal("test1", file.Packages.First().Key);
		Assert.Equal("test2", file.Packages.Skip(1).First().Key);
		//TODO Assert.Equal("test3", file.Packages.Skip(2).First().Key);
	}

	[Fact]
	public void CanUpdateProjectJsonFile()
	{
		//arrange
		var filename = Path.Combine("FileTypes", "project.json");
		var file = new ProjectJsonFile(filename);
		var results = new[]
		{
			new Result("test1", new Release(new PackageVersion(0, 1, 0, 1), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)),
			new Result("test2", new Release(new PackageVersion(0, 2, 0), DateTime.Today), new Release(new PackageVersion(2, 3, 4), DateTime.Today)),
			new Result("test3", new Release(new PackageVersion(0, 3, 0), DateTime.Today), new Release(new PackageVersion(3, 4, 5), DateTime.Today))
		};

		//act
		file.Update(results);

		//assert
		var newFile = new ProjectJsonFile(filename);
		Assert.Equal("1.2.3", newFile.Packages.First().Value!.ToString());
		Assert.Equal("2.3.4", newFile.Packages.Skip(1).First().Value!.ToString());
		//TODO Assert.Equal("3.4.5", newFile.Packages.Skip(2).First().Value!.ToString());
	}
}