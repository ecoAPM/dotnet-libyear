using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;

namespace LibYear.Lib.FileTypes
{
    public class ProjectJsonFile : IProjectFile
    {
        private string _fileContents;
        public string FileName { get; }
        public IDictionary<string, NuGetVersion> Packages { get; }
        public ProjectJsonFile(string filename)
        {
            FileName = filename;
            _fileContents = File.ReadAllText(FileName);

            var deps = JObject.Parse(_fileContents).Descendants()
                .Where(d => d.Type == JTokenType.Property && d.Path.Contains("dependencies")
                && (!d.Path.Contains("[") || d.Path.EndsWith("]"))
                && ((JProperty)d).Value.Type == JTokenType.String);

            Packages = deps.ToDictionary(p => ((JProperty)p).Name.ToString(), p => new NuGetVersion(((JProperty)p).Value.ToString()));
        }

        public void Update(IEnumerable<Result> results)
        {
            lock (_fileContents)
            {
                foreach (var result in results)
                    _fileContents = _fileContents.Replace($"\"{result.Name}\": \"{result.Installed.Version}\"", $"\"{result.Name}\": \"{result.Latest.Version}\"");

                File.WriteAllText(FileName, _fileContents);
            }
        }
    }
}