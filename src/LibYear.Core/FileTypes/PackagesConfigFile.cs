namespace LibYear.Core.FileTypes;

public class PackagesConfigFile : XmlProjectFile
{
	public PackagesConfigFile(string filename, string contents) : base(filename, contents, "package", ["id"], "version")
	{
	}
}