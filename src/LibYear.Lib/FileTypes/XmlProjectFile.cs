using System.Xml.Linq;

namespace LibYear.Lib.FileTypes;

public abstract class XmlProjectFile : IProjectFile
{
	private readonly XDocument _xmlContents;
	private readonly string _elementName;
	private readonly string[] _packageAttributeNames;
	private readonly string _versionAttributeName;
	private static readonly object _lock = new();

	public string FileName { get; }
	public IDictionary<string, PackageVersion?> Packages { get; }

	protected XmlProjectFile(string filename, string elementName, string[] packageAttributeNames, string versionAttributeName)
	{
		FileName = filename;

		_elementName = elementName;
		_packageAttributeNames = packageAttributeNames;
		_versionAttributeName = versionAttributeName;

		lock (_lock)
		{
			using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			_xmlContents = XDocument.Load(stream);
		}

		Packages = _xmlContents.Descendants(elementName).ToDictionary(
			d => packageAttributeNames.Select(p => d.Attribute(p)?.Value ?? d.Element(p)?.Value).FirstOrDefault(v => v != null)!,
			d => ParseCurrentVersion(d, versionAttributeName)
		);
	}

	public void Update(IEnumerable<Result> results)
	{
		lock (_lock)
		{
			foreach (var result in results.Where(r => r.Latest != null))
			{
				foreach (var element in GetElements(result))
					UpdateElement(element, result.Latest!.Version.ToString());
			}

			using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Write);
			new StreamWriter(stream).WriteAsync(_xmlContents.ToString()).GetAwaiter().GetResult();
		}
	}

	private static PackageVersion? ParseCurrentVersion(XElement element, string versionAttributeName)
	{
		var version = element.Attribute(versionAttributeName)?.Value ?? element.Element(versionAttributeName)?.Value ?? string.Empty;
		return !string.IsNullOrEmpty(version) ? PackageVersion.Parse(version) : null;
	}

	private void UpdateElement(XElement element, string latestVersion)
	{
		var attribute = element.Attribute(_versionAttributeName);
		if (attribute != null)
		{
			attribute.Value = latestVersion;
			return;
		}

		var e = element.Element(_versionAttributeName);
		if (e != null)
			e.Value = latestVersion;
	}

	private IEnumerable<XElement> GetElements(Result result)
	{
		return _xmlContents.Descendants(_elementName)
			.Where(d => _packageAttributeNames.Any(attributeName => (d.Attribute(attributeName)?.Value ?? d.Element(attributeName)?.Value) == result.Name
			                                                        && (d.Attribute(_versionAttributeName)?.Value ?? d.Element(_versionAttributeName)?.Value) == result.Installed?.Version.ToString()
				)
			);
	}
}