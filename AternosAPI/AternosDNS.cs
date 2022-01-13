using System.Collections.Generic;

namespace AternosAPI
{
    public class AternosDns
    {
        public string Type { get; set; }
        public List<string> Domains { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
}