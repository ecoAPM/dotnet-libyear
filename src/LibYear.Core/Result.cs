namespace LibYear.Core;

public class Result
{
	public string Name { get; }
	public Release? Installed { get; }
	public Release? Latest { get; }

	public Result(string name, Release? installed, Release? latest)
	{
		Name = name;
		Installed = installed;
		Latest = latest;
	}

	public double YearsBehind => DaysBehind > 0
		? DaysBehind / 365
		: 0;

	private double DaysBehind => (Latest?.Date - Installed?.Date ?? TimeSpan.Zero).TotalDays;
}