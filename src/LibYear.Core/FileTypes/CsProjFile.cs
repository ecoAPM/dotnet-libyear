namespace LibYear.Core.FileTypes;

public class CsProjFile(string filename, string contents) : XmlProjectFile(filename, contents, "PackageReference", ["Include", "Update"], "Version");