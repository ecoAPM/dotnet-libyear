using System.IO.Abstractions;
using LibYear.Core.FileTypes;
using Xunit;

namespace LibYear.Core.Tests.FileTypes;

public class FileSystemExtensionsTests
{
	[Fact]
	public void CanMatchCsProjFile()
	{
		//arrange
		var fileSystem = new FileSystem();
		var matchInfo = fileSystem.FileInfo.New("test.csproj");
		var noMatchInfo = fileSystem.FileInfo.New("test.vbproj");

		//act
		var isMatch = matchInfo.IsCsProjFile();
		var isNotMatch = noMatchInfo.IsCsProjFile();

		//assert
		Assert.True(isMatch);
		Assert.False(isNotMatch);
	}

	[Fact]
	public void CanMatchPackagesConfigFile()
	{
		//arrange
		var fileSystem = new FileSystem();
		var matchInfo = fileSystem.FileInfo.New("packages.config");
		var noMatchInfo = fileSystem.FileInfo.New("sausages.config");

		//act
		var isMatch = matchInfo.IsNuGetFile();
		var isNotMatch = noMatchInfo.IsNuGetFile();

		//assert
		Assert.True(isMatch);
		Assert.False(isNotMatch);
	}

	[Fact]
	public void CanMatchMSBuildPropsFile()
	{
		//arrange
		var fileSystem = new FileSystem();
		var matchInfo = fileSystem.FileInfo.New("Directory.Build.props");
		var noMatchInfo = fileSystem.FileInfo.New("Directory.Build.NoCops");

		//act
		var isMatch = matchInfo.IsMSBuildPropsFile();
		var isNotMatch = noMatchInfo.IsMSBuildPropsFile();

		//assert
		Assert.True(isMatch);
		Assert.False(isNotMatch);
	}

	[Fact]
	public void CanMatchMSBuildTargetsFile()
	{
		//arrange
		var fileSystem = new FileSystem();
		var matchInfo = fileSystem.FileInfo.New("Directory.Build.targets");
		var noMatchInfo = fileSystem.FileInfo.New("Directory.destroy.targets");

		//act
		var isMatch = matchInfo.IsMSBuildTargetsFile();
		var isNotMatch = noMatchInfo.IsMSBuildTargetsFile();

		//assert
		Assert.True(isMatch);
		Assert.False(isNotMatch);
	}

	[Fact]
	public void CanMatchCentralPackageManagementFile()
	{
		//arrange
		var fileSystem = new FileSystem();
		var matchInfo = fileSystem.FileInfo.New("Directory.Packages.props");
		var noMatchInfo = fileSystem.FileInfo.New("Directory.Packages.targets");

		//act
		var isMatch = matchInfo.IsCentralPackageManagementFile();
		var isNotMatch = noMatchInfo.IsCentralPackageManagementFile();

		//assert
		Assert.True(isMatch);
		Assert.False(isNotMatch);
	}
}