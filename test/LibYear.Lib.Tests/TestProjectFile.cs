using System.Collections.Generic;
using LibYear.Lib.FileTypes;
using NuGet.Versioning;

namespace LibYear.Lib.Tests
{
    public class TestProjectFile : IProjectFile
    {
        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }

        public TestProjectFile(string fileName, IDictionary<string, NuGetVersion> packages = null)
        {
            FileName = fileName;
            Packages = packages;
        }

        public void Update(IEnumerable<Result> results)
        {
        }
    }
}