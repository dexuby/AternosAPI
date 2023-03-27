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
            var token = File.ReadAllText(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "aternos_token.txt"));
            _aternosClient = new AternosClient(token);
        }

        [TestMethod]
        public async Task TestPreparing()
        {
            var response = await _aternosClient.PrepareAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());
        }

        [TestMethod]
        public async Task TestServerLog()
        {
            var response = await _aternosClient.PrepareAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            var serverIdResponse = await _aternosClient.GetServerIdAsync();
            Assert.IsTrue(serverIdResponse.Succeeded());
            var serverId = serverIdResponse.GetValue();
            Assert.IsNotNull(serverId);

            _aternosClient.SelectServer(serverId);

            var logResponse = await _aternosClient.GetSelectedServerLogAsync();
            Assert.IsTrue(logResponse.Succeeded());
            var log = logResponse.GetValue();
            Assert.IsNotNull(log);
        }

        [TestMethod]
        public async Task TestListAdding()
        {
            var response = await _aternosClient.PrepareAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            response = await _aternosClient.UpdateServerIdAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            response = await _aternosClient.AddPlayerToListAsync(AternosList.Whitelist, "x");
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());
        }

        [TestMethod]
        public async Task TestListRemoving()
        {
            var response = await _aternosClient.PrepareAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            response = await _aternosClient.UpdateServerIdAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            response = await _aternosClient.RemovePlayerFromListAsync(AternosList.Whitelist, "x");
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());
        }

        [TestMethod]
        public async Task TestLastServerStatus()
        {
            var response = await _aternosClient.PrepareAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            response = await _aternosClient.UpdateServerIdAsync();
            Assert.IsTrue(response.Succeeded());
            Assert.IsTrue(response.GetValue());

            var serverStatusResponse = await _aternosClient.GetLastServerStatusAsync();
            Assert.IsTrue(serverStatusResponse.Succeeded());
            Assert.IsNotNull(serverStatusResponse.GetValue().Name);
        }
    }
}