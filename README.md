# AternosAPI
[![NuGet version](https://badge.fury.io/nu/AternosAPI.svg)](https://badge.fury.io/nu/AternosAPI)

Unofficial aternos.org client to control your server(s).

## Wiki: https://github.com/dexuby/AternosAPI/wiki

Example usage:
```c#
var aternosClient = new AternosClient("<YOUR ATERNOS TOKEN>");
var response = await aternosClient.PrepareAsync();
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to fetch ajax token!");
  return;
}

// Auto select server
response = await aternosClient.UpdateServerIdAsync();
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to update sever id!");
  return;
}

// Start selected server
response = await aternosClient.StartSelectedServerAsync();
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to start the selected server!");
  return;
}

// Stop selected server
response = await aternosClient.StopSelectedServerAsync();
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to stop the selected server!");
  return;
}

// Restart selected server
response = await aternosClient.RestartSelectedServerAsync();
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to restart the selected server!");
  return;
}

// Change a setting (max player count in this example)
response = await aternosClient.ChangeSelectedServerConfigAsync("/server.properties", "max-players", "50");
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to change config!");
  return;
}

// Add a player to a list
response = await aternosClient.AddPlayerToListAsync(AternosList.Whitelist, "test");
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to add player to whitelist!");
  return;
}

// Remove a player from a list
response = await aternosClient.RemovePlayerFromListAsync(AternosList.Whitelist, "test");
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to remove player from whitelist!");
  return;
}

// Install software
response = await aternosClient.InstallSoftwareAsync("<SOFTWARE ID>", false);
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to install software!");
  return;
}

// Install plugin
response = await aternosClient.InstallPluginAsync(AternosPluginProvider.Spigot, "<PLUGIN ID>", "<VERSION ID>");
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to install plugin!");
  return;
}

// Delete file
response = await aternosClient.DeleteFileAsync("/plugins/Example.jar");
if (response.Failed() || !response.GetValue())
{
  Console.WriteLine("Failed to delete file!");
  return;
}

// Get last server status
var statusResponse = await aternosClient.GetLastServerStatusAsync();
if (statusResponse.Succeeded())
{
  var lastStatus = statusResponse.GetValue();
  Console.WriteLine($"Server name: {lastStatus.Name} | Server version: {lastStatus.Version} | Online players: {lastStatus.Players}");
}

// Get AternosLog instance
var logResponse = await aternosClient.GetSelectedServerLogAsync();
if (logResponse.Failed())
{
  Console.WriteLine("Failed to fetch log object!");
  return;
}
```

If you want to run the unit tests create a text file called "aternos_token.txt" in your application data directory (%appdata%) with your token inside.
