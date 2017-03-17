using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Versioning;

namespace LibYear.FileTypes
{
    public abstract class XmlProjectFile : IProjectFile
    {
        private readonly string _elementName;
        private readonly string _packageAttributeName;
        private readonly string _versionAttributeName;
        private readonly XDocument _xmlContents;

        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }

        protected XmlProjectFile(string filename, string elementName, string packageAttributeName, string versionAttributeName)
        {
            FileName = filename;
            _xmlContents = XDocument.Load(FileName);

            _elementName = elementName;
            _packageAttributeName = packageAttributeName;
            _versionAttributeName = versionAttributeName;

            Packages = _xmlContents.Descendants(_elementName)
                .ToDictionary(d => d.Attribute(_packageAttributeName).Value, d => new NuGetVersion(d.Attribute(_versionAttributeName).Value));
        }

        public void Update(IEnumerable<Result> results)
        {
            foreach (var result in results)
            {
                var elements = _xmlContents.Descendants(_elementName)
                    .Where(d => d.Attribute(_packageAttributeName).Value == result.Name && d.Attribute(_versionAttributeName).Value == result.Installed.Version.ToString());

                foreach (var element in elements)
                    element.Attribute(_versionAttributeName).Value = result.Latest.Version.ToString();
            }

            lock (_xmlContents)
                File.WriteAllText(FileName, _xmlContents.ToString());
        }
    }
}