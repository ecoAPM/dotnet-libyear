using Xunit;

namespace LibYear.Core.Tests;

public class ResultTests
{
	[Fact]
	public void YearsBehindCalculatedCorrectly()
	{
		//arrange
		var installed = new Release(new PackageVersion(1, 0, 0), new DateTime(2015, 1, 1));
		var latest = new Release(new PackageVersion(2, 0, 0), new DateTime(2016, 1, 1));

		//act
		var result = new Result("test", installed, latest);
		var yearsBehind = result.YearsBehind;

		//assert
		Assert.Equal(1, yearsBehind);
	}

	[Fact]
	public void YearsBehindIsZeroWhenNoInstalledVersionFound()
	{
		//arrange
		var latest = new Release(new PackageVersion(2, 0, 0), new DateTime(2016, 1, 1));

		//act
		var result = new Result("test", null, latest);
		var yearsBehind = result.YearsBehind;

		//assert
		Assert.Equal(0, yearsBehind);
	}

	[Fact]
	public void YearsBehindIsZeroWhenNoLatestVersionFound()
	{
		//arrange
		var installed = new Release(new PackageVersion(2, 0, 0), new DateTime(2016, 1, 1));

		//act
		var result = new Result("test", installed, null);
		var yearsBehind = result.YearsBehind;

		//assert
		Assert.Equal(0, yearsBehind);
	}

	[Fact]
	public void YearsBehindIsZeroWhenCurrentVersionIsNewerThanLatest()
	{
		//arrange
		var installed = new Release(new PackageVersion("2.0.0-preview1"), new DateTime(2016, 1, 1));
		var latest = new Release(new PackageVersion(1, 0, 0), new DateTime(2015, 1, 1));

		//act
		var result = new Result("test", installed, latest);
		var yearsBehind = result.YearsBehind;

		//assert
		Assert.Equal(0, yearsBehind);
	}
}