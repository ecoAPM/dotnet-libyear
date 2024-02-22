using LibYear.Core.FileTypes;
using Xunit;

namespace LibYear.Core.Tests.FileTypes;

public class DirectoryPackagesPropsTests
{
	[Fact]
	public async Task CanLoadDirectoryPackagesPropsFile()
	{
		//arrange
		var filename = Path.Combine("FileTypes", "Directory.Packages.props");

		//act
		var file = new DirectoryPackagesPropsFile(filename, await File.ReadAllTextAsync(filename));

		//assert
		Assert.Equal("test1", file.Packages.First().Key);
		Assert.Equal("test2", file.Packages.Skip(1).First().Key);
		Assert.Equal("test3", file.Packages.Skip(2).First().Key);
	}

	[Fact]
	public async Task CanUpdateDirectoryPackagesPropsFile()
	{
		//arrange
		var filename = Path.Combine("FileTypes", "Directory.Packages.props");
		var file = new DirectoryPackagesPropsFile(filename, await File.ReadAllTextAsync(filename));
		var results = new[]
		{
			new Result("test1", new Release(new PackageVersion(0, 1, 0), DateTime.Today), new Release(new PackageVersion(1, 2, 3), DateTime.Today)),
			new Result("test2", new Release(new PackageVersion(0, 2, 0), DateTime.Today), new Release(new PackageVersion(2, 3, 4), DateTime.Today)),
			new Result("test3", new Release(new PackageVersion(0, 3, 0), DateTime.Today), new Release(new PackageVersion(3, 4, 5), DateTime.Today))
		};

		//act
		var updated = file.Update(results);

		//assert
		var newFile = new DirectoryPackagesPropsFile(filename, updated);
		Assert.Equal("1.2.3", newFile.Packages.First().Value!.ToString());
		Assert.Equal("2.3.4", newFile.Packages.Skip(1).First().Value!.ToString());
		Assert.Equal("3.4.5", newFile.Packages.Skip(2).First().Value!.ToString());
	}

	[Fact]
	public async Task InvalidVersionShowsInfo()
	{
		//arrange
		var filename = Path.Combine("FileTypes", "Directory.Packages.props");
		var contents = await File.ReadAllTextAsync(filename);
		var newContents = contents.Replace("0.2.0", "0.2.x");

		try
		{
			//act
			_ = new DirectoryPackagesPropsFile(filename, newContents);
			Assert.False(true);
		}
		catch (Exception e)
		{
			//assert
			Assert.Contains("0.2.x", e.Message);
			Assert.Contains("test2", e.Message);
			Assert.Contains("Directory.Packages.props", e.Message);
		}
	}
}