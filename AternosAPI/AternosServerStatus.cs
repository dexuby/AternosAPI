using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AternosAPI
{
    public class AternosServerStatus
    {
        public string Brand { get; set; }
        public int Status { get; set; }
        public long Change { get; set; }
        public int Slots { get; set; }
        public int Problems { get; set; }
        public int Players { get; set; }
        public List<string> PlayerList { get; set; }
        public AternosMessage Message { get; set; }
        public string DynIp { get; set; }
        public bool Bedrock { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Headstarts { get; set; }
        public int Ram { get; set; }
        public string Lang { get; set; }
        public string Label { get; set; }
        public string Class { get; set; }
        public string Countdown { get; set; }
        public string Queue { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Software { get; set; }
        public string SoftwareId { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public bool Deprecated { get; set; }
        public string Ip { get; set; }
        public string DisplayAddress { get; set; }
        [JsonPropertyName("motd")] public string MessageOfTheDay { get; set; }
        public bool OnlineMode { get; set; }
        public string Icon { get; set; }
        public AternosDns Dns { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}