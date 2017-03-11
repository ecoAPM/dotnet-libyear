using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NuGet.Versioning;

namespace LibYear
{
    public class CsProjFile : IProjectFile
    {
        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }

        public CsProjFile(string filename)
        {
            FileName = filename;
            Packages = XDocument.Load(filename).Descendants("PackageReference")
                .ToDictionary(d => d.Attribute("Include").Value, d => new NuGetVersion(d.Attribute("Version").Value));
        }
    }
}