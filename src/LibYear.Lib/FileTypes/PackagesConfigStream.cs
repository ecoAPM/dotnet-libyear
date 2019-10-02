using System.IO;

namespace LibYear.Lib.FileTypes
{
    public class PackagesConfigStream : XmlProject
    {
        public PackagesConfigStream(Stream fileStream) : base(fileStream, "package", new string[] { "id" }, "version")
        {
        }
    }
}