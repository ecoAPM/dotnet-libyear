using Xunit;

namespace LibYear.Core.Tests;

public class PackageVersionTests
{
	[Fact]
	public void CanCreatePackageVersion()
	{
		var version = new PackageVersion(1, 2, 3, 4);

		Assert.Equal("1.2.3.4", version.ToString());
	}

	[Fact]
	public void CanParseSemVerString()
	{
		var version = new PackageVersion("1.2.3");

		Assert.Equal("1.2.3", version.ToString());
	}

	[Fact]
	public void CanParseNuGetString()
	{
		var version = new PackageVersion("1.2.3.4");

		Assert.Equal("1.2.3.4", version.ToString());
	}

	[Fact]
	public void CanParseWildcardString()
	{
		var version = new PackageVersion("*");

		Assert.Equal("0.0.0", version.ToString());
		Console.WriteLine(version.Wildcard);
		Assert.Equal(PackageVersion.WildcardPosition.Major, version.Wildcard);
	}

	[Fact]
	public void CanParseFormattedString()
	{
		var version = new PackageVersion("1.2.3");

		Assert.Equal("1.2.3", version.ToString("N", null));
	}
}