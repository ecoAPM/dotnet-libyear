namespace LibYear.Core.FileTypes;

public class MSBuildPropsFile(string filename, string contents) : XmlProjectFile(filename, contents, "PackageReference", ["Include", "Update"], "Version");