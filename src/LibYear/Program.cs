using System.Collections.Concurrent;
using LibYear.Lib;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace LibYear.App;

public static class Program
{
	public static void Main(string[] args)
	{
		try
		{
			var metadataResource = new SourceRepository(new PackageSource("https://api.nuget.org/v3/index.json"), Repository.Provider.GetCoreV3()).GetResource<PackageMetadataResource>();
			var versionCache = new ConcurrentDictionary<string, IList<Release>>();
			var packageVersionChecker = new PackageVersionChecker(metadataResource, versionCache);
			var projectRetriever = new ProjectFileManager();

			var runner = new Runner(packageVersionChecker, projectRetriever);
			var output = runner.Run(new List<string>(args));
			Console.WriteLine(output);
		}
		catch (Exception e)
		{
			Console.WriteLine("Sorry, an unexpected exception has occurred: " + e);
		}
	}
}