using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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