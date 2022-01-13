namespace AternosAPI
{
    public class AternosPluginProvider : ValueHolder<string>
    {
        public static AternosPluginProvider Spigot = new("spigot");
        public static AternosPluginProvider Bukkit = new("bukkit");
        public static AternosPluginProvider ArtemisBukkit = new("artemis-bukkit");

        private AternosPluginProvider(string value) : base(value)
        {
        }
    }
}