namespace LibYear.Core.FileTypes;

public class DirectoryBuildPropsFile : XmlProjectFile
{
	public DirectoryBuildPropsFile(string filename, string contents) : base(filename, contents, "PackageReference", new[] { "Include", "Update" }, "Version")
	{
	}
}