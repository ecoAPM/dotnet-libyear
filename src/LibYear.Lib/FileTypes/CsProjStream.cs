using System.IO;

namespace LibYear.Lib.FileTypes
{
    public class CsProjStream : XmlProject
    {
        public CsProjStream(Stream fileStream) : base(fileStream, "PackageReference", new string[] { "Include", "Update" }, "Version")
        {
        }
    }
}