namespace LibYear.Core.FileTypes;

public class MSBuildPropsFile : XmlProjectFile
{
	public MSBuildPropsFile(string filename, string contents) : base(filename, contents, "PackageReference", ["Include", "Update"], "Version")
	{
	}
}