using System;
using System.Linq;
using MessagePack;
using MessagePack.Formatters;
using ProGaudi.Tarantool.Client.Utils;
using static MessagePack.MessagePackBinary;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
    public class AuthenticationRequest : IRequest
    {
        private AuthenticationRequest(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }

        public CommandCodes Code => CommandCodes.Auth;

        public static AuthenticationRequest Create(GreetingsResponse greetings, UriBuilder uri)
        {
            var scrable = GetScrable(greetings, uri.Password);
            var authenticationPacket = new AuthenticationRequest(uri.UserName, scrable);
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

        internal class Formatter : IMessagePackFormatter<AuthenticationRequest>
        {
            public int Serialize(ref byte[] bytes, int offset, AuthenticationRequest value, IFormatterResolver formatterResolver)
            {
                if (value == null) return WriteNil(ref bytes, offset);

                var startOffset = offset;

                offset += WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
                offset += WriteUInt32(ref bytes, offset, Keys.Username);
                offset += WriteString(ref bytes, offset, value.Username);
                offset += WriteUInt32(ref bytes, offset, Keys.Tuple);
                offset += WriteArrayHeader(ref bytes, offset, 2);
                offset += WriteString(ref bytes, offset, "chap-sha1");
                offset += WriteBytes(ref bytes, offset, value.Scramble);

                return offset - startOffset;
            }

            public AuthenticationRequest Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new NotImplementedException();
            }
        }
    }
}