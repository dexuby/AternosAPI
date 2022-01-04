# AternosAPI
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

// Get AternosLog instance
var log = await aternosClient.GetSelectedServerLogAsync();
if (log == null) {
  Console.WriteLine("Failed to fetch log object!");
  return;
}
```

If you want to run the unit tests create a text file called "aternos_token.txt" in your application data directory (%appdata%) with your token inside.
