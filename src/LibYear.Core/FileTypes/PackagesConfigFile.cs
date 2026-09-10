namespace LibYear.Core.FileTypes;

public class PackagesConfigFile(string filename, string contents) : XmlProjectFile(filename, contents, "package", ["id"], "version");