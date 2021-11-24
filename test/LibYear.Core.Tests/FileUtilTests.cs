using LibYear.Core.FileTypes;
using Xunit;

namespace LibYear.Core.Tests;

public class FileUtilTests
{
	[Fact]
	public void RecognizesCsProjFiles()
	{
		Assert.IsType<CsProjFile>(Path.Combine("FileTypes", "project.csproj").ToProjectFile());
	}

	[Fact]
	public void RecognizesDirectoryBuildPropsFiles()
	{
		Assert.IsType<DirectoryBuildPropsFile>(Path.Combine("FileTypes", "Directory.Build.props").ToProjectFile());
	}

	[Fact]
	public void RecognizesDirectoryBuildTargetsFiles()
	{
		Assert.IsType<DirectoryBuildTargetsFile>(Path.Combine("FileTypes", "Directory.Build.targets").ToProjectFile());
	}

	[Fact]
	public void RecognizesNuGetFiles()
	{
		Assert.IsType<PackagesConfigFile>(Path.Combine("FileTypes", "packages.config").ToProjectFile());
	}

	[Fact]
	public void UnrecognizedFilesReturnNull()
	{
		Assert.Null("xunit.runner.json".ToProjectFile());
	}
}