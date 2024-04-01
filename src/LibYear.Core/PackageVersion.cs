using NuGet.Versioning;

namespace LibYear.Core;

public sealed class PackageVersion : NuGetVersion
{
	public WildcardType WildcardType { get; }

	public PackageVersion(string version)
		: this(ParseVersionString(version))
	{
	}

	public PackageVersion(PackageVersion version)
		: base(version)
	{
		WildcardType = version.WildcardType;
	}

	public PackageVersion(NuGetVersion version)
		: this(version, WildcardType.None)
	{
	}

	public PackageVersion(NuGetVersion version, WildcardType wildcardType)
		: base(version)
	{
		WildcardType = wildcardType;
	}

	public PackageVersion(int major, int minor, int patch)
		: base(major, minor, patch)
	{
		WildcardType = WildcardType.None;
	}

	public PackageVersion(int major, int minor, int patch, int revision)
		: base(major, minor, patch, revision)
	{
		WildcardType = WildcardType.None;
	}

	private static PackageVersion ParseVersionString(string versionString)
	{
		var wildcard = GetWildcardPosition(versionString);

		if (wildcard == WildcardType.None)
		{
			return new PackageVersion(new NuGetVersion(versionString), wildcard);
		}

		var lastPeriod = versionString.LastIndexOf('.');
		var version = wildcard == WildcardType.Major
			? new NuGetVersion("0")
			: new NuGetVersion(versionString[..lastPeriod]);

		return new PackageVersion(version, wildcard);
	}

	private static WildcardType GetWildcardPosition(string version)
		=> (version.Contains('*'), version.Count(f => f == '.')) switch
		{
			(true, 0) => WildcardType.Major,
			(true, 1) => WildcardType.Minor,
			(true, 2) => WildcardType.Patch,
			(true, 3) => WildcardType.Revision,
			_ => WildcardType.None
		};

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