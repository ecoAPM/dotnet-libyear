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

	[Theory]
	[InlineData("1.2.3.4", "1.2.3.4", WildcardType.None)]
	[InlineData("1.2.3", "1.2.3", WildcardType.None)]
	[InlineData("1.2", "1.2", WildcardType.None)]
	[InlineData("1", "1", WildcardType.None)]
	[InlineData("1.2.3.*", "1.2.3", WildcardType.Revision)]
	[InlineData("1.2.*", "1.2", WildcardType.Patch)]
	[InlineData("1.*", "1", WildcardType.Minor)]
	[InlineData("*", "0", WildcardType.Major)]
	public void CanParseFormattedString(string versionString, string expected, WildcardType wildcard)
	{
		var version = new PackageVersion(versionString);

		Assert.Equal(expected, version.ToString());
		Assert.Equal(wildcard, version.WildcardType);
	}
}