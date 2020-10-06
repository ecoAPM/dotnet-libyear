using System;
using NuGet.Versioning;

public class PackageVersion : NuGetVersion
{
    public bool IsWildcard { get; private set; }

    public PackageVersion(string version)
        : this(Parse(version))
    {
    }

    public PackageVersion(bool isWildcard)
        : this(0,0,0)
    {
        IsWildcard = isWildcard;
    }

    public PackageVersion(NuGetVersion version)
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

    public new static PackageVersion Parse(string version)
    {
        if (version.Equals("*"))
        {
            return new PackageVersion(true);
        }

        return new PackageVersion(NuGetVersion.Parse(version));
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(OriginalVersion) || IsSemVer2)
        {
            return ToString("N", VersionFormatter.Instance);
        }

        return OriginalVersion;
    }

    public new virtual string ToString(string format, IFormatProvider formatProvider)
    {

        if (formatProvider == null
            || !TryFormatter(format, formatProvider, out string formattedString))
        {
            formattedString = ToString();
        }

        return formattedString;
    }

    protected new bool TryFormatter(string format, IFormatProvider formatProvider, out string formattedString)
    {
        var formatted = false;
        formattedString = null;

        if (formatProvider != null)
        {
            if (formatProvider is ICustomFormatter formatter)
            {
                formatted = true;
                formattedString = formatter.Format(format, this, formatProvider);
            }
        }

        return formatted;
    }
}