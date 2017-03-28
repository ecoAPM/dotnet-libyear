using System.Collections.Generic;

namespace LibYear.Lib.FileTypes
{
    public interface IProjectFile : IHavePackages
    {
        string FileName { get; }
        void Update(IEnumerable<Result> results);
    }
}