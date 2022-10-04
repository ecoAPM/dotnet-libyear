using Xunit;

namespace LibYear.Core.Tests;

public class SolutionResultTests
{
	[Fact]
	public void DaysBehindIsTotalFromProjects()
	{
		//arrange
		var project1 = new TestProjectFile("test1", new Dictionary<string, PackageVersion?>());
		var project2 = new TestProjectFile("test2", new Dictionary<string, PackageVersion?>());
		var projects = new[]
		{
			new ProjectResult(project1, new[]
			{
				new Result("dep1", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 1)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5))),
				new Result("dep2", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 2)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5)))
			}),
			new ProjectResult(project2, new[]
			{
				new Result("dep3", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 3)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5))),
				new Result("dep4", new Release(new PackageVersion("1.2.3"), new DateTime(2022, 10, 4)), new Release(new PackageVersion("1.2.4"), new DateTime(2022, 10, 5)))
			})
		};
		var result = new SolutionResult(projects);

		//act
		var behind = result.DaysBehind;

		//assert
		Assert.Equal(10, behind);
	}
}