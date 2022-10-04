namespace LibYear.Core;

public class Result : HasAgeMeasurements
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

	public override double DaysBehind
		=> (Latest?.Date - Installed?.Date ?? TimeSpan.Zero).TotalDays;
}