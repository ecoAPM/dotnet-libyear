using System;
using NuGet.Versioning;

namespace LibYear.Lib
{
	public class PackageVersion : NuGetVersion
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
			if (formatProvider is ICustomFormatter formatter)
			{
				formattedString = formatter.Format(format, this, formatProvider);
				return true;
			}

			formattedString = null;
			return false;
		}
	}
}