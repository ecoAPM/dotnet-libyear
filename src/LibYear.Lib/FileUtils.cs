using System.IO;

namespace LibYear.Lib
{
    public static class FileUtils
    {
        public static bool IsCsProjFile(this string path) => File.Exists(path) && new FileInfo(path).Extension == ".csproj";
        public static bool IsProjectJsonFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "project.json";
        public static bool IsNuGetFile(this string path) => File.Exists(path) && new FileInfo(path).Name == "packages.config";
    }
}