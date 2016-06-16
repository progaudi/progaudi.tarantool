using System;

using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Services
{
    public class AuthenticationRequestFactory
    {
        public AuthenticationPacket CreateAuthentication(GreetingsResponse greetings, string username, string password)
        {
            var scrable = GetScrable(greetings, password);
            var authenticationPacket = new AuthenticationPacket(username, scrable);
            return authenticationPacket;
        }

        private static byte[] GetScrable(GreetingsResponse greetings, string password)
        {
            var decodedSalt = greetings.Salt;
            var first20SaltBytes = new byte[20];
            Array.Copy(decodedSalt, first20SaltBytes, 20);

            var step1 = Sha1Utils.Hash(password);
            var step2 = Sha1Utils.Hash(step1);
            var step3 = Sha1Utils.Hash(first20SaltBytes, step2);
            var scrambleBytes = Sha1Utils.Xor(step1, step3);

            return scrambleBytes;
        }
    }
}