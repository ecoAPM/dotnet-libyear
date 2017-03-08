using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibYear
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var projects = getProjects(args);
            foreach (var project in projects)
            {
                project.ShowDependencies().GetAwaiter().GetResult();
            }
        }

        private static IEnumerable<string> getProjects(string[] args)
        {
            if (!args.Any())
                return getProjects(Directory.GetCurrentDirectory());

            if (args[0].IsDirectory())
                return getProjects(args[0]);

            if (args[0].IsProjectFile())
                return new[] {args[0]};

            throw new FileNotFoundException("No C# projects found in ");
        }

        private static IEnumerable<string> getProjects(string dirPath)
        {
            var dir = new DirectoryInfo(dirPath);
            var csproj = dir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
            return csproj != null
                ? new[] { csproj.FullName }
                : dir.GetFiles("*.csproj", SearchOption.AllDirectories).Select(f => f.FullName);
        }
    }
}