using LibYear.Core;

namespace LibYear;

public class LimitChecker
{
	private readonly Settings _settings;

	public LimitChecker(Settings settings)
		=> _settings = settings;

	public bool AnyLimitsExceeded(SolutionResult result)
		=> TotalLimitExceeded(result)
		   || AnyProjectLimitExceeded(result)
		   || AnyDependencyLimitExceeded(result);

	private bool AnyDependencyLimitExceeded(SolutionResult result)
		=> _settings.LimitAny != null && result.Details.Any(r => r.Details.Any(p => p.YearsBehind > _settings.LimitAny));

	private bool AnyProjectLimitExceeded(SolutionResult result)
		=> _settings.LimitProject != null && result.Details.Any(r => r.Details.Sum(p => p.YearsBehind) > _settings.LimitProject);

	private bool TotalLimitExceeded(SolutionResult allResults)
		=> _settings.LimitTotal != null && allResults.YearsBehind > _settings.LimitTotal;
}