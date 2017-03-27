using System.Collections.Generic;
using NuGet.Versioning;

namespace LibYear.Lib.FileTypes
{
    public interface IProjectFile
    {
        string FileName { get; }
        IDictionary<string, NuGetVersion> Packages { get; }
        void Update(IEnumerable<Result> results);
    }
}