using System;

namespace AternosAPI
{
    public class ServerIdNotFoundException : Exception
    {
        public ServerIdNotFoundException()
        {
        }

        public ServerIdNotFoundException(string message) : base(message)
        {
        }

        public ServerIdNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}