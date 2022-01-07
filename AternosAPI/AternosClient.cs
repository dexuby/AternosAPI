using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RequesterLib;

namespace AternosAPI
{
    public class AternosClient
    {
        private readonly Requester _requester;
        private string _token;

        public AternosClient(string session)
        {
            _requester = new Requester.Builder()
                .WithDefaultHeader("User-Agent", "Mozilla/5.0")
                .WithCookie("aternos.org", "ATERNOS_SESSION", session)
                .Build();
        }

        public async Task<bool> PrepareAsync(int maxAttempts = 200)
        {
            try
            {
                var content = await _requester.GetStringAsync(Constants.AccountUrl);

                var attempt = 1;
                while (!Regex.IsMatch(content, Constants.TokenPartsPattern) && attempt < maxAttempts)
                {
                    content = await _requester.GetStringAsync(Constants.AccountUrl);
                    attempt++;
                }

                if (!Regex.IsMatch(content, Constants.TokenPartsPattern)) return false;

                var token = Regex.Match(content, Constants.TokenPartsPattern).Groups[1].Value;
                _token = token;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SelectServer(string id)
        {
            if (_requester.HasCookie("http://www.aternos.org", "ATERNOS_SERVER"))
                _requester.UpdateCookie("http://www.aternos.org", "ATERNOS_SERVER", id);
            else _requester.AddCookie("aternos.org", "ATERNOS_SERVER", id);
        }

        public async Task<bool> UpdateServerIdAsync()
        {
            var serverId = await GetServerIdAsync();
            if (string.IsNullOrWhiteSpace(serverId)) return false;
            SelectServer(serverId);
            return true;
        }

        public async Task<string> GetServerIdAsync()
        {
            try
            {
                var content = await _requester.GetStringAsync("https://aternos.org/servers/");
                if (!Regex.IsMatch(content, Constants.ServerIdPattern)) return null;
                var serverId = Regex.Match(content, Constants.ServerIdPattern).Groups[1].Value;

                return string.IsNullOrWhiteSpace(serverId) ? null : serverId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> StartSelectedServerAsync(int headStart = 0, int accessCredits = 0)
        {
            try
            {
                var response = await _requester.GetJsonElementAsync(
                    PrepareRequest(
                        $"https://aternos.org/panel/ajax/start.php?headStart={headStart}&access-credits={accessCredits}"));
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> StopSelectedServerAsync()
        {
            try
            {
                var response =
                    await _requester.GetJsonElementAsync(PrepareRequest("https://aternos.org/panel/ajax/stop.php"));
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RestartSelectedServerAsync()
        {
            try
            {
                var response =
                    await _requester.GetAsync(PrepareRequest("https://aternos.org/panel/ajax/restart.php"));
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangeSelectedServerConfigAsync(string file, string option, string value)
        {
            try
            {
                var response =
                    await _requester.PostStringContentAsync(
                        PrepareRequest("https://aternos.org/panel/ajax/options/config.php"),
                        $"file={file}&option={option}&value={value}");
                var json = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync())).RootElement;
                return json.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AternosLog> GetSelectedServerLogAsync()
        {
            try
            {
                return await _requester.GetFromJsonAsync<AternosLog>(
                    PrepareRequest("https://aternos.org/panel/ajax/mclogs.php"));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AddPlayerToListAsync(string list, string name)
        {
            try
            {
                var response =
                    await _requester.PostStringContentAsync(
                        PrepareRequest("https://aternos.org/panel/ajax/players/add.php"),
                        $"list={list}&name={name}");
                var json = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync())).RootElement;
                return json.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddPlayerToListAsync(AternosList list, string name) =>
            await AddPlayerToListAsync(list.GetValue(), name);

        public async Task<bool> RemovePlayerFromListAsync(string list, string name)
        {
            try
            {
                var response =
                    await _requester.PostStringContentAsync(
                        PrepareRequest("https://aternos.org/panel/ajax/players/remove.php"),
                        $"list={list}&name={name}");
                var json = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync())).RootElement;
                return json.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemovePlayerFromListAsync(AternosList list, string name) =>
            await RemovePlayerFromListAsync(list.GetValue(), name);

        public async Task<bool> InstallSoftwareAsync(string softwareId, bool reinstall)
        {
            try
            {
                var response =
                    await _requester.GetAsync(PrepareRequest(
                        $"https://aternos.org/panel/ajax/software/install.php?software={softwareId}&reinstall={(reinstall ? 1 : 0)}"));
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InstallPluginAsync(string provider, string pluginId, string versionId)
        {
            try
            {
                var response =
                    await _requester.PostStringContentAsync(
                        PrepareRequest("https://aternos.org/panel/ajax/players/remove.php"),
                        $"provider={provider}&addon={pluginId}&version={versionId}");
                var json = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync())).RootElement;
                return json.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> InstallPluginAsync(AternosPluginProvider provider, string pluginId, string versionId) =>
            await InstallPluginAsync(provider.GetValue(), pluginId, versionId);

        private string PrepareRequest(string url)
        {
            var name = AternosUtils.GenerateRandomString();
            var value = AternosUtils.GenerateRandomString();

            _requester.ExpireCookies("http://www.aternos.org", cookie => cookie.Name.StartsWith("ATERNOS_SEC"));
            _requester.AddCookie("aternos.org", $"ATERNOS_SEC_{name}", value);

            return new StringBuilder(url)
                .Append(url.Contains("?") ? "&" : "?")
                .Append($"SEC={name}%3A{value}&TOKEN={_token}")
                .ToString();
        }

        public string GetAjaxToken => _token;
    }
}