using LibYear.Core;
using LibYear.Core.Tests;
using Xunit;

namespace LibYear.Tests;

public class LimitCheckerTests
{
	private static readonly SolutionResult SolutionResult = new([
		new ProjectResult(new TestProjectFile("test1", new Dictionary<string, PackageVersion?>()), [
			new Result("dep1", new Release(new PackageVersion("1.2.3"), new DateTime(2018, 1, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 1, 1))),
			new Result("dep2", new Release(new PackageVersion("1.2.3"), new DateTime(2019, 1, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 1, 1)))
		]),
		new ProjectResult(new TestProjectFile("test1", new Dictionary<string, PackageVersion?>()), [
			new Result("dep3", new Release(new PackageVersion("1.2.3"), new DateTime(2020, 1, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 1, 1))),
			new Result("dep4", new Release(new PackageVersion("1.2.3"), new DateTime(2021, 1, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 1, 1)))
		])
	]);

	[Fact]
	public void PassesWhenNoLimitsSet()
	{
		//arrange
		var settings = new Settings();
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.True(passed);
	}

	[Fact]
	public void PassesWhenUnderTotalLimit()
	{
		//arrange
		var settings = new Settings { LimitTotal = 11 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.True(passed);
	}

	[Fact]
	public void FailsWhenOverTotalLimit()
	{
		//arrange
		var settings = new Settings { LimitTotal = 9 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.False(passed);
	}

	[Fact]
	public void PassesWhenUnderProjectLimit()
	{
		//arrange
		var settings = new Settings { LimitProject = 8 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.True(passed);
	}

	[Fact]
	public void FailsWhenOverProjectLimit()
	{
		//arrange
		var settings = new Settings { LimitProject = 4 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.False(passed);
	}

	[Fact]
	public void PassesWhenUnderAnyLimit()
	{
		//arrange
		var settings = new Settings { LimitAny = 4.5 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.True(passed);
	}

	[Fact]
	public void FailsWhenOverAnyLimit()
	{
		//arrange
		var settings = new Settings { LimitAny = 0.5 };
		var checker = new LimitChecker(settings);

		//act
		var passed = !checker.AnyLimitsExceeded(SolutionResult);

		//assert
		Assert.False(passed);
	}
}