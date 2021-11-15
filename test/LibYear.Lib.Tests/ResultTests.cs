using System;
using Xunit;

namespace LibYear.Lib.Tests;

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
}