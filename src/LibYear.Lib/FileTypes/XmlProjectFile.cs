using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibYear.Lib.FileTypes
{
    public abstract class XmlProjectFile : XmlProject
    {
        private readonly string _elementName;
        private readonly string _packageAttributeName;
        private readonly string _versionAttributeName;
        public override string FileName { get; }

        protected XmlProjectFile(string filename, string elementName, string packageAttributeName, string versionAttributeName)
            :base (File.OpenRead(filename), elementName, packageAttributeName, versionAttributeName)
        {
            FileName = filename;

            _elementName = elementName;
            _packageAttributeName = packageAttributeName;
            _versionAttributeName = versionAttributeName;

            // Don't like this, but don't want to do it in the base class
            // as I would prefer consumers handle the lifetime of the stream
            _underlyingStreamData.Dispose();
        }

        public override void Update(IEnumerable<Result> results)
        {
            lock (_xmlContents)
            {
                foreach (var result in results)
                {
                    var elements = _xmlContents.Descendants(_elementName)
                        .Where(d => (d.Attribute(_packageAttributeName)?.Value ?? d.Element(_packageAttributeName)?.Value) == result.Name &&
                                    (d.Attribute(_versionAttributeName)?.Value ?? d.Element(_versionAttributeName)?.Value) == result.Installed.Version.ToString());

                    foreach (var element in elements)
                    {
                        if(element.Attribute(_versionAttributeName) != null)
                            element.Attribute(_versionAttributeName).Value = result.Latest.Version.ToString();
                        else
                        {
                            var e = element.Element(_versionAttributeName);
                            if (e != null)
                                e.Value = result.Latest.Version.ToString();
                        }
                    }
                }

                File.WriteAllText(FileName, _xmlContents.ToString());
            }
        }
    }
}