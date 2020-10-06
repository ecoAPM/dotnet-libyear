using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace LibYear.Lib.Tests
{
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

            var checker = new PackageVersionChecker(metadataResource, new Dictionary<string, IList<Release>>());

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
                .Returns(m => new List<IPackageSearchMetadata> { metadata }, m => throw new Exception(":("));

            var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
            var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
            var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
            var checker = new PackageVersionChecker(metadataResource, versionCache);

            //act
            var result = await checker.GetResultTask("test", new PackageVersion(1, 2, 3));

            //assert
            var latest = result.Latest.Version.ToString();
            Assert.Equal("2.3.4", latest);
        }

        [Fact]
        public async Task InstalledVersionEqualsLatestVersionWithWildcard()
        {
            //arrange
            var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", PackageVersion.Parse("*"))).Build();
            var metadataResource = Substitute.For<PackageMetadataResource>();
            metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
                .Returns(m => new List<IPackageSearchMetadata> { metadata }, m => throw new Exception(":("));

            var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
            var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
            var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
            var checker = new PackageVersionChecker(metadataResource, versionCache);

            //act
            var result = await checker.GetResultTask("test", PackageVersion.Parse("*"));

            //assert
            var installed = result.Installed.Version;
            var latest = result.Latest.Version;
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
                .Returns(m => new List<IPackageSearchMetadata> { metadata }, m => throw new Exception(":("));

            var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
            var v2 = new Release(PackageVersion.Parse("2.3.4-beta-1"), new DateTime(2016, 1, 1));
            var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
            var checker = new PackageVersionChecker(metadataResource, versionCache);

            //act
            var result = await checker.GetResultTask("test", new PackageVersion(1, 2, 3));

            //assert
            var latest = result.Latest.Version.ToString();
            Assert.Equal("1.2.3", latest);
        }

        [Fact]
        public async Task LatestDoesNotIncludeUnpublished()
        {
            //arrange
            var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
            var metadataResource = Substitute.For<PackageMetadataResource>();
            metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
                .Returns(m => new List<IPackageSearchMetadata> { metadata }, m => throw new Exception(":("));

            var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
            var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1), false);
            var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
            var checker = new PackageVersionChecker(metadataResource, versionCache);

            //act
            var result = await checker.GetResultTask("test", new PackageVersion(1, 2, 3));

            //assert
            var latest = result.Latest.Version.ToString();
            Assert.Equal("1.2.3", latest);
        }

        [Fact]
        public void AwaitResultsWaitsForAll()
        {
            //arrange
            var resultsTasks = new[]
            {
                Sleep(1),
                Sleep(2),
                Sleep(3),
                Sleep(4)
            };

            //act
            var versions = PackageVersionChecker.AwaitResults(resultsTasks).ToArray();

            //assert
            Assert.Equal("1", versions[0].Name);
            Assert.Equal("2", versions[1].Name);
            Assert.Equal("3", versions[2].Name);
            Assert.Equal("4", versions[3].Name);
        }

        #pragma warning disable 1998
        private static async Task<Result> Sleep(int x)
        {
            Thread.Sleep(x);
            return new Result(x.ToString(), null, null);
        }
        #pragma warning restore 1998

        [Fact]
        public void GetPackagesGetsPackages()
        {
            //arrange
            var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new PackageVersion(1, 2, 3))).Build();
            var metadataResource = Substitute.For<PackageMetadataResource>();
            metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<SourceCacheContext>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>())
                .Returns(m => new List<IPackageSearchMetadata> { metadata }, m => throw new Exception(":("));

            var v1 = new Release(new PackageVersion(1, 2, 3), new DateTime(2015, 1, 1));
            var v2 = new Release(new PackageVersion(2, 3, 4), new DateTime(2016, 1, 1));
            var versionCache = new Dictionary<string, IList<Release>> { { "test", new List<Release> { v1, v2 } } };
            var checker = new PackageVersionChecker(metadataResource, versionCache);
            var project = new TestProjectFile("test", new Dictionary<string, PackageVersion> { { "test", new PackageVersion(1, 2, 3) } });

            //act
            var packages = checker.GetPackages(new[] { project });

            //assert
            var latest = packages.First().Value.First().Latest.Version.ToString();
            Assert.Equal("2.3.4", latest);

        }
    }
}
