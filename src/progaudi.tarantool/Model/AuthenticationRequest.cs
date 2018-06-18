using System;
using System.Linq;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
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

        internal class Formatter : IMsgPackConverter<AuthenticationRequest>
        {
            private IMsgPackConverter<uint> _keyConverter;
            private IMsgPackConverter<byte[]> _bytesConverter;
            private IMsgPackConverter<string> _stringConverter;
            private IMsgPackConverter<object> _nullConverter;

            public void Initialize(MsgPackContext context)
            {
                _keyConverter = context.GetConverter<uint>();
                _bytesConverter = context.GetConverter<byte[]>();
                _stringConverter = context.GetConverter<string>();
                _nullConverter = context.NullConverter;
            }

            public void Write(AuthenticationRequest value, IMsgPackWriter writer)
            {
                if (value == null)
                {
                    _nullConverter.Write(null, writer);
                    return;
                }

                writer.WriteMapHeader(2);

                _keyConverter.Write(Keys.Username, writer);
                _stringConverter.Write(value.Username, writer);

                _keyConverter.Write(Keys.Tuple, writer);

                writer.WriteArrayHeader(2);
                _stringConverter.Write("chap-sha1", writer);
                _bytesConverter.Write(value.Scramble, writer);
            }

            public AuthenticationRequest Read(IMsgPackReader reader)
            {
                throw new NotImplementedException();
            }
        }
    }
}