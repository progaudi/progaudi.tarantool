using System;
using System.Buffers;
using System.Linq;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class AuthenticationRequest : IRequest, IDisposable
    {
        private AuthenticationRequest(string username, IMemoryOwner<byte> scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public IMemoryOwner<byte> Scramble { get; }

        public CommandCode Code => CommandCode.Auth;

        public static AuthenticationRequest Create(GreetingsResponse greetings, UriBuilder uri)
        {
            var scramble = GetScramble(greetings, uri.Password);
            return new AuthenticationRequest(uri.UserName, scramble);
        }

        private static IMemoryOwner<byte> GetScramble(GreetingsResponse greetings, string password)
        {
            var first20SaltBytes = greetings.Salt.AsSpan(0, 20);

            var step1 = Sha1Utils.Hash(password);
            var step2 = Sha1Utils.Hash(step1);
            var step3 = Sha1Utils.Hash(first20SaltBytes, step2);
            var owner = MemoryPool<byte>.Shared.Rent(step1.Length);
            Sha1Utils.Xor(step1, step3, owner.Memory.Span);

            return owner;
        }

        public override string ToString() => $"Username: {Username}, Scramble: {string.Join(" ", Scramble.Memory.ToArray().Select(x => x.ToString("x2")))}";

        public void Dispose()
        {
            Scramble?.Dispose();
        }
    }
}