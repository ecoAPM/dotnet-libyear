using System.Xml.Linq;

namespace LibYear.Core.FileTypes;

public abstract class XmlProjectFile : IProjectFile
{
	public string FileName { get; }
	public IDictionary<string, PackageVersion?> Packages { get; }

	private readonly XDocument _xmlContents;
	private readonly string _elementName;
	private readonly string[] _packageAttributeNames;
	private readonly string _versionAttributeName;

	protected XmlProjectFile(string filename, string contents, string elementName, string[] packageAttributeNames, string versionAttributeName)
	{
		FileName = filename;

		_elementName = elementName;
		_packageAttributeNames = packageAttributeNames;
		_versionAttributeName = versionAttributeName;

		_xmlContents = XDocument.Parse(contents);
		Packages = _xmlContents.Descendants(elementName)
			.ToDictionary(
				d => packageAttributeNames.Select(p => d.Attribute(p)?.Value ?? d.Element(p)?.Value).FirstOrDefault(v => v != null)!,
				d => ParseCurrentVersion(d, versionAttributeName)
			);
	}

	public string Update(IReadOnlyCollection<Result> results)
	{
		foreach (var result in results.Where(r => r.Latest != null))
		{
			foreach (var element in GetMatchingElements(result))
				UpdateElement(element, result.Latest!.Version.ToString());
		}

		return _xmlContents.ToString();
	}

	private PackageVersion? ParseCurrentVersion(XElement element, string versionAttributeName)
	{
		var version = element.Attribute(versionAttributeName)?.Value ?? element.Element(versionAttributeName)?.Value ?? string.Empty;
		try
		{
			return !string.IsNullOrEmpty(version)
				? new PackageVersion(version)
				: null;
		}
		catch (Exception e)
		{
			throw new ArgumentException($"Could not parse version {version} of {element} in {FileName}", e);
		}
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

	private XElement[] GetMatchingElements(Result result)
		=> _xmlContents.Descendants(_elementName)
			.Where(d => _packageAttributeNames.Any(attributeName => (d.Attribute(attributeName)?.Value ?? d.Element(attributeName)?.Value) == result.Name
					&& (d.Attribute(_versionAttributeName)?.Value ?? d.Element(_versionAttributeName)?.Value) == result.Installed?.Version.ToString()
				)
			)
			.ToArray();
}