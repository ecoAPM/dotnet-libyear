using System;

namespace LibYear.Lib
{
    public class Result
    {
        public string Name { get; }
        public Release Installed { get; }
        public Release Latest { get; }

        public Result(string name, Release installed, Release latest)
        {
            Name = name;
            Installed = installed;
            Latest = latest;
        }

        public double YearsBehind => (Latest?.Date - Installed?.Date ?? TimeSpan.Zero).TotalDays / 365;
    }
}