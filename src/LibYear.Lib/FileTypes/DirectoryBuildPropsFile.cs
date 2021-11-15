namespace LibYear.Lib.FileTypes;

public class DirectoryBuildPropsFile : XmlProjectFile
{
	public DirectoryBuildPropsFile(string filename) : base(filename, "PackageReference", new[] { "Include", "Update" }, "Version")
	{
	}
}
