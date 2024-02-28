namespace LibYear.Core.FileTypes;

public class MSBuildTargetsFile : XmlProjectFile
{
	public MSBuildTargetsFile(string filename, string contents) : base(filename, contents, "PackageReference", new[] { "Include", "Update" }, "Version")
	{
	}
}