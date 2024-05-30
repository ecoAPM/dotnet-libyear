using LibYear.Core;

namespace LibYear.Output;

public interface IOutput
{
	public void DisplayAllResults(SolutionResult allResults, bool quietMode);
}