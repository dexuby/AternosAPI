namespace AternosAPI
{
    public class AternosList : ValueHolder<string>
    {
        public static AternosList Whitelist = new("whitelist");
        public static AternosList Ops = new("ops");
        public static AternosList BannedPlayers = new("banned-players");
        public static AternosList BannedIps = new("banned-ips");

        public AternosList(string value) : base(value)
        {
        }
    }
}