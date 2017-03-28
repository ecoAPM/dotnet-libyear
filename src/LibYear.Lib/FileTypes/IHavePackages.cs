using System.Collections.Generic;
using NuGet.Versioning;

namespace LibYear.Lib.FileTypes
{
    public interface IHavePackages
    {
        IDictionary<string, SemanticVersion> Packages { get; }
    }
}