using LibYear.Core;

namespace LibYear;

public class LimitChecker(Settings settings)
{
	public bool AnyLimitsExceeded(SolutionResult result)
		=> TotalLimitExceeded(result)
		   || AnyProjectLimitExceeded(result)
		   || AnyDependencyLimitExceeded(result);

	private bool AnyDependencyLimitExceeded(SolutionResult result)
		=> settings.LimitAny != null && result.Details.Any(r => r.Details.Any(p => p.YearsBehind > settings.LimitAny));

	private bool AnyProjectLimitExceeded(SolutionResult result)
		=> settings.LimitProject != null && result.Details.Any(r => r.Details.Sum(p => p.YearsBehind) > settings.LimitProject);

	private bool TotalLimitExceeded(SolutionResult allResults)
		=> settings.LimitTotal != null && allResults.YearsBehind > settings.LimitTotal;
}