using System.IO.Abstractions;
using LibYear.Core;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Spectre.Console;

namespace LibYear;

public static class Factory
{
	public static App App(IAnsiConsole console)
	{
		var packageVersionChecker = new PackageVersionChecker(PackageMetadataResource());
		var fileSystem = new FileSystem();
		var projectRetriever = new ProjectFileManager(fileSystem);
		return new App(packageVersionChecker, projectRetriever, console);
	}

	private static PackageMetadataResource PackageMetadataResource()
	{
		var source = new PackageSource("https://api.nuget.org/v3/index.json");
		var provider = Repository.Provider.GetCoreV3();
		var repo = new SourceRepository(source, provider);
		return repo.GetResource<PackageMetadataResource>();
	}
}