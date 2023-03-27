namespace AternosAPI
{
    public class AternosClientOptions
    {
        public static AternosClientOptions DefaultOptions = new()
        {
            UserAgent = Constants.DefaultUserAgent
        };

        public string UserAgent { get; init; }
    }
}