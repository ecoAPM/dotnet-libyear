using System;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace LibYear.Lib
{
    public class VersionInfo
    {
        public SemanticVersion Version { get; }
        public DateTime Released { get; }

        public VersionInfo(IPackageSearchMetadata metadata) : this(metadata.Identity.Version, metadata.Published.GetValueOrDefault().Date)
        {
        }

        public VersionInfo(SemanticVersion version, DateTime released)
        {
            Version = version;
            Released = released;
        }
    }
}