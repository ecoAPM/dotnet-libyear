using Newtonsoft.Json.Linq;

namespace LibYear.Core.FileTypes;

public class ProjectJsonFile : IProjectFile
{
	private string _fileContents;
	public string FileName { get; }
	public IDictionary<string, PackageVersion?> Packages { get; }
	private static readonly object _lock = new();

	public ProjectJsonFile(string filename)
	{
		FileName = filename;

		lock (_lock)
		{
			using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			_fileContents = new StreamReader(stream).ReadToEndAsync().GetAwaiter().GetResult();
		}

		Packages = JObject.Parse(_fileContents).Descendants()
			.Where(d => d.Type == JTokenType.Property
			            && d.Path.Contains("dependencies")
			            && (!d.Path.Contains("[") || d.Path.EndsWith("]"))
			            && ((JProperty)d).Value.Type == JTokenType.String)
			.ToDictionary(
				p => ((JProperty)p).Name,
				p => PackageVersion.Parse(((JProperty)p).Value.ToString())
			);
	}

	public void Update(IEnumerable<Result> results)
	{
		lock (_lock)
		{
			foreach (var result in results)
			{
				_fileContents = _fileContents.Replace($"\"{result.Name}\": \"{result.Installed?.Version}\"", $"\"{result.Name}\": \"{result.Latest?.Version}\"");
			}

			using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Write);
			new StreamWriter(stream).WriteAsync(_fileContents).GetAwaiter().GetResult();
		}
	}
}