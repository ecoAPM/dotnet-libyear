using System.Collections.Generic;

namespace LibYear.Lib.FileTypes
{
    public interface IProjectFile
    {
        string FileName { get; }
        IDictionary<string, PackageVersion> Packages { get; }
        void Update(IEnumerable<Result> results);

    }
}