namespace LibYear.Core.FileTypes;

public class MSBuildTargetsFile(string filename, string contents) : XmlProjectFile(filename, contents, "PackageReference", ["Include", "Update"], "Version");