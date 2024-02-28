namespace LibYear.Core.FileTypes;

public class CentralPackageManagementFile : XmlProjectFile
{
	public CentralPackageManagementFile(string filename, string contents) : base(filename, contents, "PackageVersion", ["Include", "Update"], "Version")
	{
	}
}