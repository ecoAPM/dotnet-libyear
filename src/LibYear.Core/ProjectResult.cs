using LibYear.Core.FileTypes;

namespace LibYear.Core;

public class ProjectResult : HasAgeMeasurements
{
	public IProjectFile ProjectFile { get; }
	public IReadOnlyCollection<Result> Details { get; }

	public ProjectResult(IProjectFile projectFile, IReadOnlyCollection<Result> details)
	{
		ProjectFile = projectFile;
		Details = details;
	}

	public override double DaysBehind
		=> Details.Sum(r => r.DaysBehind);
}