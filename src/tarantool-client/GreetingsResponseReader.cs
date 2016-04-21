using System;
using System.Text;

using iproto.Data.Packets;

namespace tarantool_client
{
    public class GreetingsResponseReader 
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;
      
        public GreetingsPacket ReadGreetings(byte[] response)
        {
            var message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            var decodedSalt= Convert.FromBase64String(saltString);

            return new GreetingsPacket(message, decodedSalt);
        }
    }
}