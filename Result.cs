using System;

namespace LibYear
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

        public double YearsBehind => (Latest.Released - Installed.Released).TotalDays / 365;
    }
}