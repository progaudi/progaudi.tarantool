using System;
using System.Text;

namespace Tarantool.Client.Model.Responses
{
    public class GreetingsResponse
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        public GreetingsResponse(byte[] response)
        {
            Message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            Salt = Convert.FromBase64String(saltString);
        }

        public string Message { get; }

        public byte[] Salt { get; }
    }
}