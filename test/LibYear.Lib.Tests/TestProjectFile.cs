using System.Collections.Generic;
using LibYear.Lib.FileTypes;
using NuGet.Versioning;

namespace LibYear.Lib.Tests
{
    public class TestProjectFile : IProjectFile
    {
        public string FileName { get; }
        public IDictionary<string, SemanticVersion> Packages { get; }

        public TestProjectFile(string fileName, IDictionary<string, SemanticVersion> packages = null)
        {
            FileName = fileName;
            Packages = packages;
        }

        public void Update(IEnumerable<Result> results)
        {
        }
    }
}