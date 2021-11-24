using NuGet.Versioning;

namespace LibYear.Core;

public sealed class PackageVersion : NuGetVersion
{
	public bool IsWildcard { get; }

	public PackageVersion(string version)
		: this(Parse(version))
	{
		if (version.Equals("*"))
		{
			IsWildcard = true;
		}

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

	private new static PackageVersion? Parse(string version)
	{
		if (version.Equals("*"))
		{
			return new PackageVersion(0, 0, 0);
		}

		try
		{
			var nuGetVersion = new NuGetVersion(version);
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