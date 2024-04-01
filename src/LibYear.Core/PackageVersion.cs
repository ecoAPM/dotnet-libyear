using System.Runtime.InteropServices;
using NuGet.Versioning;

namespace LibYear.Core;

public sealed class PackageVersion : NuGetVersion
{
	public enum WildcardPosition {
		Major,
		Minor,
		Patch,
		None
	}

	public WildcardPosition Wildcard { get; }

	public PackageVersion(string version)
		: this(Parse(version))
	{
	}

	public PackageVersion(NuGetVersion version, WildcardPosition position = WildcardPosition.None)
		: base(version)
	{
		Wildcard = position;
	}

	public PackageVersion(PackageVersion version)
		: base(version)
	{
		Wildcard = version.Wildcard;
	}

	public PackageVersion(int major, int minor, int patch)
		: base(major, minor, patch)
	{
		Wildcard = WildcardPosition.None;
	}

	public PackageVersion(int major, int minor, int patch, int revision)
		: base(major, minor, patch, revision)
	{
		Wildcard = WildcardPosition.None;
	}

	private new static PackageVersion Parse(string version)
	{
		if (version.EndsWith('*'))
		{
			WildcardPosition position = WildcardPosition.None;
			switch (version.Count(f => f == '.'))
			{
				case 0:
					return new PackageVersion(new NuGetVersion(0, 0, 0), WildcardPosition.Major);
				case 1:
					position = WildcardPosition.Minor;
					break;
				case 2:
					position = WildcardPosition.Patch;
					break;
			}
			int index = version.LastIndexOf(".");
			return new PackageVersion(new NuGetVersion(version.Substring(0, index)), position);
		}
		else
			return new PackageVersion(new NuGetVersion(version));
	}

	public override string ToString()
		=> string.IsNullOrEmpty(OriginalVersion) || IsSemVer2
			? ToString("N", VersionFormatter.Instance)
			: OriginalVersion;

	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		if (format == null || formatProvider == null || !TryFormatter(format, formatProvider, out var formattedString))
		{
			formattedString = ToString();
		}

		return formattedString;
	}

	private new bool TryFormatter(string format, IFormatProvider formatProvider, out string formattedString)
	{
		if (formatProvider is ICustomFormatter formatter)
		{
			formattedString = formatter.Format(format, this, formatProvider);
			return true;
		}

		formattedString = string.Empty;
		return false;
	}
}