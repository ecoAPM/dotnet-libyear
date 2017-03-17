using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;

namespace LibYear.FileTypes
{
    public class ProjectJsonFile : IProjectFile
    {
        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }

        public ProjectJsonFile(string filename)
        {
            FileName = filename;
            var deps = JObject.Parse(File.ReadAllText(filename)).Descendants()
                .Where(d => d.Type == JTokenType.Property && d.Path.Contains("dependencies") && !d.Path.EndsWith("dependencies"));
            Packages = deps
                .ToDictionary(p => ((JProperty)p).Name.ToString(), p => new NuGetVersion(((JProperty)p).Value.ToString()));
        }
    }
}