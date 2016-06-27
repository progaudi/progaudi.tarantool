using System;
using System.Linq;

using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Model.Requests
{
    public class AuthenticationRequest : IRequest
    {
        private AuthenticationRequest(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }

        public CommandCode Code => CommandCode.Auth;

        public static AuthenticationRequest Create(GreetingsResponse greetings, string username, string password)
        {
            var scrable = GetScrable(greetings, password);
            var authenticationPacket = new AuthenticationRequest(username, scrable);
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

        public override string ToString()
        {
            return $"Username: {Username}, Scramble: {ToReadableString(Scramble)}";
        }

        private static string ToReadableString(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("X2 ")));
        }
    }
}