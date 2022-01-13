using System;
using System.IO;
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
        private readonly AternosClientOptions _options;
        private readonly Requester _requester;
        private string _token;

        public AternosClient(string session, AternosClientOptions options = null)
        {
            _options = options ?? AternosClientOptions.DefaultOptions;
            _requester = new Requester.Builder()
                .WithDefaultHeader("User-Agent", _options.UserAgent)
                .WithCookie("aternos.org", "ATERNOS_SESSION", session)
                .Build();
        }

        public async Task<Response<bool>> PrepareAsync(int maxAttempts = 200)
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

                if (!Regex.IsMatch(content, Constants.TokenPartsPattern))
                    return Response<bool>.Failure(
                        new TokenNotFoundException($"Failed to fetch token in {maxAttempts} attempts."));

                var token = Regex.Match(content, Constants.TokenPartsPattern).Groups[1].Value;
                _token = token;

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public void SelectServer(string id)
        {
            if (_requester.HasCookie("http://www.aternos.org", "ATERNOS_SERVER"))
                _requester.UpdateCookie("http://www.aternos.org", "ATERNOS_SERVER", id);
            else _requester.AddCookie("aternos.org", "ATERNOS_SERVER", id);
        }

        public async Task<Response<bool>> UpdateServerIdAsync()
        {
            var serverIdResponse = await GetServerIdAsync();
            if (serverIdResponse.Failed() || string.IsNullOrWhiteSpace(serverIdResponse.GetValue()))
                return Response<bool>.Failure(new ServerIdNotFoundException("Failed to fetch a valid server id."));
            SelectServer(serverIdResponse.GetValue());
            return Response<bool>.Success(true);
        }

        public async Task<Response<string>> GetServerIdAsync()
        {
            try
            {
                var content = await _requester.GetStringAsync("https://aternos.org/servers/");
                if (!Regex.IsMatch(content, Constants.ServerIdPattern)) return null;
                var serverId = Regex.Match(content, Constants.ServerIdPattern).Groups[1].Value;

                return string.IsNullOrWhiteSpace(serverId)
                    ? Response<string>.Failure(new ServerIdNotFoundException("Failed to fetch a valid server id."))
                    : Response<string>.Success(serverId);
            }
            catch (Exception ex)
            {
                return Response<string>.Failure(ex);
            }
        }

        public async Task<Response<bool>> StartSelectedServerAsync(int headStart = 0, int accessCredits = 0)
        {
            try
            {
                var response = await _requester.GetJsonElementAsync(
                    PrepareRequest(
                        $"https://aternos.org/panel/ajax/start.php?headStart={headStart}&access-credits={accessCredits}"));
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> StopSelectedServerAsync()
        {
            try
            {
                var response =
                    await _requester.GetJsonElementAsync(PrepareRequest("https://aternos.org/panel/ajax/stop.php"));
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> RestartSelectedServerAsync()
        {
            try
            {
                var response =
                    await _requester.GetAsync(PrepareRequest("https://aternos.org/panel/ajax/restart.php"));
                return Response<bool>.Success(response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> ChangeSelectedServerConfigAsync(string file, string option, string value)
        {
            try
            {
                var response = await _requester.PostStringContentAndGetJsonElementAsync(
                    PrepareRequest("https://aternos.org/panel/ajax/options/config.php"),
                    $"file={file}&option={option}&value={value}");
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<AternosLog>> GetSelectedServerLogAsync()
        {
            try
            {
                var log = await _requester.GetFromJsonAsync<AternosLog>(
                    PrepareRequest("https://aternos.org/panel/ajax/mclogs.php"));
                return Response<AternosLog>.Success(log);
            }
            catch (Exception ex)
            {
                return Response<AternosLog>.Failure(ex);
            }
        }

        public async Task<Response<bool>> AddPlayerToListAsync(string list, string name)
        {
            try
            {
                var response = await _requester.PostStringContentAndGetJsonElementAsync(
                    PrepareRequest("https://aternos.org/panel/ajax/players/add.php"),
                    $"list={list}&name={name}");
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> AddPlayerToListAsync(AternosList list, string name) =>
            await AddPlayerToListAsync(list.GetValue(), name);

        public async Task<Response<bool>> RemovePlayerFromListAsync(string list, string name)
        {
            try
            {
                var response = await _requester.PostStringContentAndGetJsonElementAsync(
                    PrepareRequest("https://aternos.org/panel/ajax/players/remove.php"),
                    $"list={list}&name={name}");
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> RemovePlayerFromListAsync(AternosList list, string name) =>
            await RemovePlayerFromListAsync(list.GetValue(), name);

        public async Task<Response<bool>> InstallSoftwareAsync(string softwareId, bool reinstall)
        {
            try
            {
                var response =
                    await _requester.GetAsync(PrepareRequest(
                        $"https://aternos.org/panel/ajax/software/install.php?software={softwareId}&reinstall={(reinstall ? 1 : 0)}"));
                return Response<bool>.Success(response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> InstallPluginAsync(string provider, string pluginId, string versionId)
        {
            try
            {
                var response = await _requester.PostStringContentAndGetJsonElementAsync(
                    PrepareRequest("https://aternos.org/panel/ajax/installaddon.php"),
                    $"provider={provider}&addon={pluginId}&version={versionId}");
                return Response<bool>.Success(response.GetProperty("success").GetBoolean());
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<bool>> InstallPluginAsync(AternosPluginProvider provider, string pluginId,
            string versionId) =>
            await InstallPluginAsync(provider.GetValue(), pluginId, versionId);

        public async Task<Response<bool>> DeleteFileAsync(string path)
        {
            try
            {
                var response =
                    await _requester.PostStringContentAsync(
                        PrepareRequest("https://aternos.org/panel/ajax/delete.php"),
                        $"file={path}");
                return Response<bool>.Success(response.StatusCode == HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure(ex);
            }
        }

        public async Task<Response<AternosServerStatus>> GetLastServerStatusAsync()
        {
            try
            {
                var response = await _requester.GetStringAsync(Constants.ServerUrl);
                var json = Regex.Match(response, Constants.LastServerStatusPattern).Groups[1].Value;
                await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var serverStatus =
                    await JsonSerializer.DeserializeAsync<AternosServerStatus>(stream,
                        Constants.AternosJsonSerializerOptions);
                return Response<AternosServerStatus>.Success(serverStatus);
            }
            catch (Exception ex)
            {
                return Response<AternosServerStatus>.Failure(ex);
            }
        }

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

        public string GetAjaxToken() => _token;

        public AternosClientOptions GetOptions() => _options;
    }
}