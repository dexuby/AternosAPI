using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AternosAPI
{
    public class AternosList
    {
        public static AternosList Whitelist = new("whitelist");
        public static AternosList Ops = new("ops");
        public static AternosList BannedPlayers = new("banned-players");
        public static AternosList BannedIps = new("banned-ips");

        private readonly string _value;

        private AternosList(string value)
        {
            _value = value;
        }

        public string GetValue() => _value;
    }
}
