namespace LibYear.Lib.FileTypes
{
  public class PackagesConfigFile : XmlProjectFile
  {
    public PackagesConfigFile(string filename) : base(filename, "package", new string[] { "id" }, "version")
    {
    }
  }
}