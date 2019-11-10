namespace LibYear.Lib.FileTypes
{
    public class DirectoryBuildTargetsFile : XmlProjectFile
    {
        public DirectoryBuildTargetsFile(string filename) : base(filename, "PackageReference", new[] { "Include", "Update" }, "Version")
        {
        }
    }
}