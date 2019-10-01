namespace LibYear.Lib.FileTypes
{
  public class CsProjFile : XmlProjectFile
  {
    public CsProjFile(string filename) : base(filename, "PackageReference", new string[] { "Include", "Update" }, "Version")
    {
    }
  }
}