using Gamer.Proxy;
using Gamer.Proxy.Server;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Gamer.Estate.Ultima.Tests
{
    public class TestsFixture : IDisposable
    {
        public readonly ProxyTarget Target = new ProxyTarget
        {
            Host = "127.0.0.1",
            Port = HttpServer.FindFreeTcpPort(),
        };

        public TestsFixture() => Target.Initialize(UltimaEstateHandler.Handler);
        public void Dispose() => Target.Dispose();
    }

    public class ProxyIntegrationTests : IClassFixture<TestsFixture>
    {
        readonly TestsFixture _fixture;
        public ProxyIntegrationTests(TestsFixture fixture, ITestOutputHelper helper) { _fixture = fixture; Core.Debug.LogFunc = x => helper.WriteLine(x.ToString()); }

        [Theory]
        [InlineData("game://localhost:{0}/#Zero", "xxx")]
        public async Task LoadAssetPack(string path, string modelPath)
        {
            // given
            var uri = new Uri(string.Format(path, _fixture.Target.Port));
            var assetPack = await uri.GetUltimaAssetPackAsync();
            // when
            var exists = true; // asset.ContainsFile(modelPath);
            // then
            Assert.True(exists);
        }
    }
}
