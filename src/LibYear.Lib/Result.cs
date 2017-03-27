using System;

namespace LibYear.Lib
{
    public class Result
    {
        public string Name { get; }
        public VersionInfo Installed { get; }
        public VersionInfo Latest { get; }

        public Result(string name, VersionInfo installed, VersionInfo latest)
        {
            Name = name;
            Installed = installed;
            Latest = latest;
        }

        public double YearsBehind => (Latest?.Released - Installed?.Released ?? TimeSpan.Zero).TotalDays / 365;
    }
}