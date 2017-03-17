namespace LibYear.FileTypes
{
    public class CsProjFile : XmlProjectFile
    {
        public CsProjFile(string filename) : base(filename, "PackageReference", "Include", "Version")
        {
        }
    }
}