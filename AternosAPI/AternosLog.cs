using System.Text.Json;

namespace AternosAPI
{
    public class AternosLog
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Raw { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}