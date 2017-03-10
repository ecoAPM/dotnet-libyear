using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace LibYear.Tests
{
    public class PackageVersionCheckerTests
    {
        [Fact]
        public async Task CanGetVersionInfoFromMetadata()
        {
            //arrange
            var metadataResource = Substitute.For<PackageMetadataResource>();
            var metadata = PackageSearchMetadataBuilder.FromIdentity(new PackageIdentity("test", new NuGetVersion(1, 2, 3))).Build();
            metadataResource.GetMetadataAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<ILogger>(), Arg.Any<CancellationToken>()).Returns(new List<IPackageSearchMetadata> { metadata });
            var checker = new PackageVersionChecker(metadataResource);

            //act
            var versions = await checker.GetVersions("test");

            //assert
            Assert.Equal("1.2.3.0", versions.First().Version.ToString());
        }
    }
}
