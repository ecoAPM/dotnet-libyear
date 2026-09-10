namespace LibYear.Core.FileTypes;

public class CentralPackageManagementFile(string filename, string contents) : XmlProjectFile(filename, contents, "PackageVersion", ["Include", "Update"], "Version");