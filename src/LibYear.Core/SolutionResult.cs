namespace LibYear.Core;

public class SolutionResult : HasAgeMeasurements
{
	public IReadOnlyCollection<ProjectResult> Details { get; }

	public SolutionResult(IReadOnlyCollection<ProjectResult> details)
		=> Details = details;

	public override double DaysBehind
		=> Details.Sum(r => r.DaysBehind);
}