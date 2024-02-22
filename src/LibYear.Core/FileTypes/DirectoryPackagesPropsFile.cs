namespace LibYear.Core.FileTypes;

public class DirectoryPackagesPropsFile : XmlProjectFile
{
	public DirectoryPackagesPropsFile(string filename, string contents) : base(filename, contents, "PackageVersion", new[] { "Include", "Update" }, "Version")
	{
	}
}