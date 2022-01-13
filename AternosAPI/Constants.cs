using System.Text.Json;

namespace AternosAPI
{
    public class Constants
    {
        public const string TokenPartsPattern = "\\.join\\(''\\)]=\"(.*)\";}\\)";
        public const string ServerIdPattern = "class=\"server-body\" data-id=\"(.*)\">";
        public const string LastServerStatusPattern = "lastStatus = ({.*});";

        public const string AccountUrl = "https://aternos.org/account/";
        public const string ServerUrl = "https://aternos.org/server/";

        public static JsonSerializerOptions AternosJsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}