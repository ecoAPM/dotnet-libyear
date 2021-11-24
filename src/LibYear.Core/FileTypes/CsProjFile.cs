namespace LibYear.Core.FileTypes;

public class CsProjFile : XmlProjectFile
{
	public CsProjFile(string filename, string contents) : base(filename, contents, "PackageReference", new[] { "Include", "Update" }, "Version")
	{
	}
}