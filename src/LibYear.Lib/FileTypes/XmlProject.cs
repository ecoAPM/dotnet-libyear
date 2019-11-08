using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace LibYear.Lib.FileTypes
{
	public abstract class XmlProject : IHavePackages
	{
		protected readonly XDocument _xmlContents;
		protected readonly Stream _xmlStream;
		public IDictionary<string, SemanticVersion> Packages { get; } = new Dictionary<string, SemanticVersion>();

		protected XmlProject(Stream fileStream, string elementName, string packageAttributeName, string versionAttributeName)
		{
			_xmlStream = fileStream;
			_xmlContents = XDocument.Load(fileStream);

			foreach (var reference in _xmlContents.Descendants(elementName))
			{
				var name = reference.Attribute(packageAttributeName)?.Value ?? reference.Element(packageAttributeName)?.Value;
				var version = reference.Attribute(versionAttributeName)?.Value ?? reference.Element(versionAttributeName)?.Value;

				if (!string.IsNullOrEmpty(name) && SemanticVersion.TryParse(version, out var semver))
				{
					Packages.Add(name, semver);
				}
				else if (System.Diagnostics.Debugger.IsAttached)
				{
					if (string.IsNullOrEmpty(name))
						Console.Error.WriteLine($"Package Reference with no name!");
					else
						Console.Error.WriteLine($"Reference \"{name}\" has missing or invalid version: \"{version ?? "null"}\".");
				}
			}
		}
	}
}