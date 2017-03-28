using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Versioning;

namespace LibYear.Lib.FileTypes
{
    public abstract class XmlProject : IProjectFile
    {
        protected readonly XDocument _xmlContents;
        protected readonly Stream _underlyingStreamData;
        public virtual string FileName => throw new NotSupportedException();
        public IDictionary<string, NuGetVersion> Packages { get; protected set; }

        public XmlProject(Stream fileStream, string elementName, string packageAttributeName, string versionAttributeName)
        {
            _underlyingStreamData = fileStream;
            _xmlContents = XDocument.Load(fileStream);

            Packages = _xmlContents.Descendants(elementName)
                .ToDictionary(d => d.Attribute(packageAttributeName)?.Value ?? d.Element(packageAttributeName)?.Value,
                    d => new NuGetVersion(d.Attribute(versionAttributeName)?.Value ?? d.Element(versionAttributeName)?.Value));
        }

        public virtual void Update(IEnumerable<Result> results) => throw new NotSupportedException();
    }
}