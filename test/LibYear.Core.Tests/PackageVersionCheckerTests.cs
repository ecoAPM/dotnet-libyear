using NSubstitute;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace LibYear.Core.Tests;

public class PackageVersionCheckerTests
{
	[Fact]
	public async Task CanGetVersionInfoFromMetadata()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(new List<IPackageSearchMetadata> { metadata });

		var checker = new PackageVersionChecker(metadataResource);

		//act
		var versions = await checker.GetVersions("test");

		//assert
		Assert.Equal("1.2.3", versions.First().Version.ToString());
	}

	[Fact]
	public async Task CanGetResultFromCache()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(_ => new List<IPackageSearchMetadata> { metadata }, _ => throw new Exception(":("));

		var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
		var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
		var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
		var checker = new PackageVersionChecker(metadataResource, versionCache);

		//act
		var result = await checker.GetResult("test", new PackageVersion(1, 2, 3));

		//assert
		var latest = result.Latest!.Version.ToString();
		Assert.Equal("2.3.4", latest);
	}

	[Fact]
	public async Task InstalledVersionEqualsLatestVersionWithWildcard()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion("*"))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(_ => new List<IPackageSearchMetadata> { metadata }, _ => throw new Exception(":("));

		var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
		var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
		var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
		var checker = new PackageVersionChecker(metadataResource, versionCache);

		//act
		var result = await checker.GetResult("test", new PackageVersion("*"));

		//assert
		var installed = result.Installed!.Version;
		var latest = result.Latest!.Version;
		Assert.Equal("2.3.4", latest.ToString());
		Assert.Equal(latest, installed);
	}

	[Fact]
	public async Task LatestDoesNotIncludePrerelease()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(_ => new List<IPackageSearchMetadata> { metadata }, _ => throw new Exception(":("));

		var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
		var v2 = new Release(new PackageVersion("2.3.4-beta-1"), new DateTime(2016, 1, 1));
		var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
		var checker = new PackageVersionChecker(metadataResource, versionCache);

		//act
		var result = await checker.GetResult("test", new PackageVersion(1, 2, 3));

		//assert
		var latest = result.Latest!.Version.ToString();
		Assert.Equal("1.2.3", latest);
	}

	[Fact]
	public async Task LatestDoesNotIncludeUnpublished()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(_ => new List<IPackageSearchMetadata> { metadata }, _ => throw new Exception(":("));

		var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
		var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1), false);
		var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
		var checker = new PackageVersionChecker(metadataResource, versionCache);

		//act
		var result = await checker.GetResult("test", new PackageVersion(1, 2, 3));

		//assert
		var latest = result.Latest!.Version.ToString();
		Assert.Equal("1.2.3", latest);
	}

	[Fact]
	public void GetPackagesGetsPackages()
	{
		//arrange
		var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
		var metadataResource = Substitute.For<PackageMetadataResource>();
		metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
			.Returns(_ => new List<IPackageSearchMetadata> { metadata }, _ => throw new Exception(":("));

		var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
		var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
		var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
		var checker = new PackageVersionChecker(metadataResource, versionCache);
		var project = new TestProjectFile("test", new Dictionary<string, PackageVersion?> { { "test", new PackageVersion(1, 2, 3) } });

		//act
		var packages = checker.GetPackages(new[] { project });

		//assert
		var latest = packages.First().Value.First().Latest!.Version.ToString();
		Assert.Equal("2.3.4", latest);

	}
}