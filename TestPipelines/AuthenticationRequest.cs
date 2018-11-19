using System;
using System.Buffers;
using System.Linq;
using ProGaudi.Buffers;

namespace TestPipelines
{
    public class AuthenticationRequest : IDisposable
    {
        private AuthenticationRequest(string username, IMemoryOwner<byte> scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public IMemoryOwner<byte> Scramble { get; }

        public CommandCode Code => CommandCode.Auth;

        public static AuthenticationRequest Create(GreetingsResponse greetings, TarantoolNode node)
        {
            var scramble = GetScramble(greetings, node.Pwd);
            return new AuthenticationRequest(node.User, scramble);
        }

        public static AuthenticationRequest Create(GreetingsResponse greetings, string username, string password)
        {
            var scramble = GetScramble(greetings, password);
            return new AuthenticationRequest(username, scramble);
        }
        
        private static IMemoryOwner<byte> GetScramble(GreetingsResponse greetings, string password)
        {
            var first20SaltBytes = greetings.Salt.AsSpan(0, 20);

            var step1 = Sha1Utils.Hash(password);
            var step2 = Sha1Utils.Hash(step1);
            var step3 = Sha1Utils.Hash(first20SaltBytes, step2);
            var owner = FixedLengthMemoryPool<byte>.Shared.Rent(step1.Length);
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