namespace LibYear.Core;

public class SolutionResult(IReadOnlyCollection<ProjectResult> details) : HasAgeMeasurements
{
	public IReadOnlyCollection<ProjectResult> Details { get; } = details;

	public override double DaysBehind
		=> Details.Sum(r => r.DaysBehind);
}