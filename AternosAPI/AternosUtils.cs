using System;

namespace AternosAPI
{
    public class AternosUtils
    {
        private static readonly Random Random = new();

        private static readonly char[] Chars =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9'
        };

        public static string GenerateRandomString()
        {
            var start = "0000000000000000".ToCharArray();
            for (var i = 0; i < 11; i++)
                start[i] = Chars[Random.Next(0, Chars.Length)];
            return new string(start);
        }
    }
}