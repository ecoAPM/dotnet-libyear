namespace LibYear.Core;

public class Result(string name, Release? installed, Release? latest) : HasAgeMeasurements
{
	public string Name { get; } = name;
	public Release? Installed { get; } = installed;
	public Release? Latest { get; } = latest;

	public override double DaysBehind
		=> (Latest?.Date - Installed?.Date ?? TimeSpan.Zero).TotalDays;
}