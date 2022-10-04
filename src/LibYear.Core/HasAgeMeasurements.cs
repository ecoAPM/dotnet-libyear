namespace LibYear.Core;

public abstract class HasAgeMeasurements
{
	public double YearsBehind => DaysBehind > 0
		? DaysBehind / 365
		: 0;

	public abstract double DaysBehind { get; }
}