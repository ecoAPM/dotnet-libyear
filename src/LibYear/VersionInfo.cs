using System;
using NuGet.Protocol.Core.Types;

namespace LibYear
{
    public class VersionInfo
    {
        public Version Version { get; }
        public DateTime Released { get; }

        public VersionInfo(IPackageSearchMetadata metadata) : this(metadata.Identity.Version.Version, metadata.Published.GetValueOrDefault().Date)
        {
        }

        public VersionInfo(Version version, DateTime released)
        {
            Version = version;
            Released = released;
        }



        public bool Matches(Version other)
        {
            return Version.Major == other.Major
                   && Version.Minor == other.Minor
                   && (Version.Build < 0 || other.Build < 0 || Version.Build == other.Build)
                   && (Version.Revision < 0 || other.Revision < 0 || Version.Revision == other.Revision);
        }
    }
}