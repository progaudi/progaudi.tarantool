using System;
using iproto.Data;
using iproto.Data.Packets;

namespace iproto.Services
{
    public class AuthenticationRequestFactory
    {
        public AuthenticationPacket CreateAuthentication(GreetingsPacket greetings, string username, string password)
        {
            var scrable = GetScrable(greetings, password);
            var header = new Header(CommandCode.Auth, 0, 0);
            var authenticationPacket = new AuthenticationPacket(header, username, scrable);
            return authenticationPacket;
        }

        private static byte[] GetScrable(GreetingsPacket greetings, string password)
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