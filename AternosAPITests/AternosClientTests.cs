using System;
using System.IO;
using System.Threading.Tasks;
using AternosAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AternosAPITests
{
    [TestClass]
    public class AternosClientTests
    {
        private readonly AternosClient _aternosClient;

        public AternosClientTests()
        {
            var token = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "aternos_token.txt"));
            _aternosClient = new AternosClient(token);
        }

        [TestMethod]
        public async Task TestPreparing()
        {
            Assert.IsTrue(await _aternosClient.PrepareAsync());
        }

        [TestMethod]
        public async Task TestServerLog()
        {
            Assert.IsTrue(await _aternosClient.PrepareAsync());
            var serverId = await _aternosClient.GetServerIdAsync();
            Assert.IsNotNull(serverId);

            _aternosClient.SelectServer(serverId);

            Assert.IsNotNull(await _aternosClient.GetSelectedServerLogAsync());
        }

        [TestMethod]
        public async Task TestListAdding()
        {
            Assert.IsTrue(await _aternosClient.PrepareAsync());
            Assert.IsTrue(await _aternosClient.UpdateServerIdAsync());
            Assert.IsTrue(await _aternosClient.AddPlayerToListAsync(AternosList.Whitelist, "x"));
        }

        [TestMethod]
        public async Task TestListRemoving()
        {
            Assert.IsTrue(await _aternosClient.PrepareAsync());
            Assert.IsTrue(await _aternosClient.UpdateServerIdAsync());
            Assert.IsTrue(await _aternosClient.RemovePlayerFromListAsync(AternosList.Whitelist, "x"));
        }

        [TestMethod]
        public async Task TestLastServerStatus()
        {
            Assert.IsTrue(await _aternosClient.PrepareAsync());
            Assert.IsTrue(await _aternosClient.UpdateServerIdAsync());
            var serverStatus = await _aternosClient.GetLastServerStatusAsync();
            Assert.IsNotNull(serverStatus.Name);
        }
    }
}