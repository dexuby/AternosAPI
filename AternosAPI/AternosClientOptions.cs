namespace AternosAPI
{
    public class AternosClientOptions
    {
        public static AternosClientOptions DefaultOptions = new()
        {
            UserAgent = "Mozilla/5.0"
        };

        public string UserAgent { get; init; }
    }
}