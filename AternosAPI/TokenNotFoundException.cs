using System;

namespace AternosAPI
{
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException()
        {
        }

        public TokenNotFoundException(string message) : base(message)
        {
        }

        public TokenNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}