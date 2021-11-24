using NuGet.Versioning;

namespace LibYear.Core;

public sealed class PackageVersion : NuGetVersion
{
	public bool IsWildcard { get; }

	public PackageVersion(string version)
		: this(Parse(version))
	{
	}

	public PackageVersion(bool isWildcard)
		: this(0, 0, 0)
	{
		IsWildcard = isWildcard;
	}

	public PackageVersion(NuGetVersion? version)
		: base(version)
	{
	}

	public PackageVersion(int major, int minor, int patch)
		: base(major, minor, patch)
	{
	}

	public PackageVersion(int major, int minor, int patch, int revision)
		: base(major, minor, patch, revision)
	{
	}

	public new static PackageVersion? Parse(string version)
	{
		if (version.Equals("*"))
		{
			return new PackageVersion(true);
		}

		try
		{
			var nuGetVersion = NuGetVersion.Parse(version);
			return new PackageVersion(nuGetVersion);
		}
		catch
		{
			return null;
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(OriginalVersion) || IsSemVer2)
		{
			return ToString("N", VersionFormatter.Instance);
		}

		return OriginalVersion;
	}

	public override string ToString(string format, IFormatProvider? formatProvider)
	{
		if (formatProvider == null
			|| !TryFormatter(format, formatProvider, out var formattedString))
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