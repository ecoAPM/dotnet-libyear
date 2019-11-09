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

        protected XmlProject(Stream fileStream, string elementName, string[] packageAttributeNames, string versionAttributeName)
        {
            _xmlStream = fileStream;
            _xmlContents = XDocument.Load(fileStream);

            Packages = _xmlContents.Descendants(elementName).ToDictionary(
                d => packageAttributeNames.Select(p => d.Attribute(p)?.Value ?? d.Element(p)?.Value).FirstOrDefault(v => v != null),
                d => SemanticVersion.Parse(d.Attribute(versionAttributeName)?.Value ?? d.Element(versionAttributeName)?.Value)
            );
        }
    }
}