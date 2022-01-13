# AternosAPI
[![NuGet version](https://badge.fury.io/nu/AternosAPI.svg)](https://badge.fury.io/nu/AternosAPI)

Unofficial aternos.org client to control your server(s).

Example usage:
```c#
var aternosClient = new AternosClient("<YOUR ATERNOS TOKEN>");
var success = await aternosClient.PrepareAsync();
if (!success) {
  Console.WriteLine("Failed to fetch ajax token!");
  return;
}

// Auto select server
success = await aternosClient.UpdateServerIdAsync();
if (!success) {
  Console.WriteLine("Failed to update sever id!");
  return;
}

// Start selected server
success = await aternosClient.StartSelectedServerAsync();
if (!success) {
  Console.WriteLine("Failed to start the selected server!");
  return;
}

// Stop selected server
success = await aternosClient.StopSelectedServerAsync();
if (!success) {
  Console.WriteLine("Failed to stop the selected server!");
  return;
}

// Restart selected server
success = await aternosClient.RestartSelectedServerAsync();
if (!success) {
  Console.WriteLine("Failed to restart the selected server!");
  return;
}

// Change a setting (max player count in this example)
success = await aternosClient.ChangeSelectedServerConfigAsync("/server.properties", "max-players", "50");
if (!success) {
  Console.WriteLine("Failed to change config!");
  return;
}

// Add a player to a list
success = await aternosClient.AddPlayerToListAsync(AternosList.Whitelist, "test");
if (!success) {
  Console.WriteLine("Failed to add player to whitelist!");
  return;
}

// Remove a player from a list
success = await aternosClient.RemovePlayerFromListAsync(AternosList.Whitelist, "test");
if (!success) {
  Console.WriteLine("Failed to remove player from whitelist!");
  return;
}

// Install software
success = await aternosClient.InstallSoftwareAsync("<SOFTWARE ID>", false);
if (!success) {
  Console.WriteLine("Failed to install software!");
  return;
}

// Install plugin
success = await aternosClient.InstallPluginAsync(AternosPluginProvider.Spigot, "<PLUGIN ID>", "<VERSION ID>");
if (!success) {
  Console.WriteLine("Failed to install plugin!");
  return;
}

// Delete file
success = await aternosClient.DeleteFileAsync("/plugins/Example.jar");
if (!success) {
  Console.WriteLine("Failed to delete file!");
  return;
}

// Get last server status
var lastStatus = await aternosClient.GetLastServerStatusAsync();
if (lastStatus != null) {
  Console.WriteLine($"Server name: {lastStatus.Name} | Server version: {lastStatus.Version} | Online players: {lastStatus.Players}");
}

// Get AternosLog instance
var log = await aternosClient.GetSelectedServerLogAsync();
if (log == null) {
  Console.WriteLine("Failed to fetch log object!");
  return;
}
```

If you want to run the unit tests create a text file called "aternos_token.txt" in your application data directory (%appdata%) with your token inside.
