using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NuGet.Versioning;

namespace LibYear.FileTypes
{
    public abstract class XmlProjectFile : IProjectFile
    {
        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }

        protected XmlProjectFile(string filename, string elementName, string packageAttributeName, string versionAttributeName)
        {
            FileName = filename;
            Packages = XDocument.Load(filename).Descendants(elementName)
                .ToDictionary(d => d.Attribute(packageAttributeName).Value, d => new NuGetVersion(d.Attribute(versionAttributeName).Value));
        }
    }
}