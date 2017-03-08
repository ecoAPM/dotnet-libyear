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
    }
}