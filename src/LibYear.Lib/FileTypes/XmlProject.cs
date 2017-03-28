using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Versioning;

namespace LibYear.Lib.FileTypes
{
    public abstract class XmlProject : IHavePackages
    {
        protected readonly XDocument _xmlContents;
        protected readonly Stream _xmlStream;
        public IDictionary<string, SemanticVersion> Packages { get; }

        protected XmlProject(Stream fileStream, string elementName, string packageAttributeName, string versionAttributeName)
        {
            _xmlStream = fileStream;
            _xmlContents = XDocument.Load(fileStream);

            Packages = _xmlContents.Descendants(elementName)
                .ToDictionary(d => d.Attribute(packageAttributeName)?.Value ?? d.Element(packageAttributeName)?.Value,
                    d => SemanticVersion.Parse(d.Attribute(versionAttributeName)?.Value ?? d.Element(versionAttributeName)?.Value));
        }
    }
}