using System.Collections.Generic;
using NuGet.Versioning;

namespace LibYear.FileTypes
{
    public interface IProjectFile
    {
        string FileName { get; }
        IDictionary<string, NuGetVersion> Packages { get; }
    }
}