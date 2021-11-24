namespace LibYear.Core.FileTypes;

public class DirectoryBuildTargetsFile : XmlProjectFile
{
	public DirectoryBuildTargetsFile(string filename, string contents) : base(filename, contents, "PackageReference", new[] { "Include", "Update" }, "Version")
	{
	}
}