using LibYear.Core.FileTypes;
using Xunit;

namespace LibYear.Core.Tests.FileTypes;

public class FileTypePatternsTests
{
	[Fact]
	public void CanMatchCsProjFiles()
	{
		//arrange
		var regex = FileTypePatterns.CSProj();

		//act
		var match = regex.IsMatch("test.csproj");
		var caseMatch = regex.IsMatch("Test.CSProj");

		//assert
		Assert.True(match);
		Assert.True(caseMatch);
	}

	[Fact]
	public void CanMatchPackageConfigFiles()
	{
		//arrange
		var regex = FileTypePatterns.PackagesConfig();

		//act
		var match = regex.IsMatch("packages.config");
		var caseMatch = regex.IsMatch("Packages.Config");

		//assert
		Assert.True(match);
		Assert.True(caseMatch);
	}

	[Fact]
	public void CanMatchDirectoryBuildPropsFiles()
	{
		//arrange
		var regex = FileTypePatterns.DirectoryBuildProps();

		//act
		var match = regex.IsMatch("Directory.Build.props");
		var caseMatch = regex.IsMatch("Directory.build.Props");

		//assert
		Assert.True(match);
		Assert.True(caseMatch);
	}

	[Fact]
	public void CanMatchDirectoryBuildTargetsFiles()
	{
		//arrange
		var regex = FileTypePatterns.DirectoryBuildTargets();

		//act
		var match = regex.IsMatch("Directory.Build.targets");
		var caseMatch = regex.IsMatch("Directory.build.Targets");

		//assert
		Assert.True(match);
		Assert.True(caseMatch);
	}

	[Fact]
	public void CanMatchCentralPackageManagementFiles()
	{
		//arrange
		var regex = FileTypePatterns.CentralPackageManagement();

		//act
		var match = regex.IsMatch("Directory.Packages.props");
		var caseMatch = regex.IsMatch("Directory.packages.Props");

		//assert
		Assert.True(match);
		Assert.True(caseMatch);
	}
}