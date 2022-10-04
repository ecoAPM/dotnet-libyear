using Xunit;

namespace LibYear.Core.Tests;

public class ProjectResultTests
{
	[Fact]
	public void DaysBehindIsTotalFromDependencies()
	{
		//arrange
		var project = new TestProjectFile("test1", new Dictionary<string, PackageVersion?>());
		var deps = new[]
		{
			new Result("dep1", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5))),
			new Result("dep2", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 2)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5)))
		};
		var result = new ProjectResult(project, deps);

		//act
		var behind = result.DaysBehind;

		//assert
		Assert.Equal(7, behind);
	}
}