using System;

using iproto.Data.Packets;
using iproto.Interfaces;
using System.Text;

namespace iproto.Services
{
    public class ResponseReader : IResponseReader
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        public UnifiedPacket ReadResponse(byte[] response)
        {
            throw new System.NotImplementedException();
        }

        public GreetingsPacket ReadGreetings(byte[] response)
        {
            var message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            var decodedSalt= Convert.FromBase64String(saltString);

            return new GreetingsPacket(message, decodedSalt);
        }
    }
}