namespace LibYear.Core.FileTypes;

public class CsProjFile : XmlProjectFile
{
	public CsProjFile(string filename, string contents) : base(filename, contents, "PackageReference", ["Include", "Update"], "Version")
	{
	}
}