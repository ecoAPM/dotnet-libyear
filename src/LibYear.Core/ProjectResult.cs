using LibYear.Core.FileTypes;

namespace LibYear.Core;

public class ProjectResult(IProjectFile projectFile, IReadOnlyCollection<Result> details) : HasAgeMeasurements
{
	public IProjectFile ProjectFile { get; } = projectFile;
	public IReadOnlyCollection<Result> Details { get; } = details;

	public override double DaysBehind
		=> Details.Sum(r => r.DaysBehind);
}