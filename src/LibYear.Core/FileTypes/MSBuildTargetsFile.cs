namespace LibYear.Core.FileTypes;

public class MSBuildTargetsFile : XmlProjectFile
{
	public MSBuildTargetsFile(string filename, string contents) : base(filename, contents, "PackageReference", ["Include", "Update"], "Version")
	{
	}
}