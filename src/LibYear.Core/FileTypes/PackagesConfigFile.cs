namespace LibYear.Core.FileTypes;

public class PackagesConfigFile : XmlProjectFile
{
	public PackagesConfigFile(string filename) : base(filename, "package", new[] { "id" }, "version")
	{
	}
}