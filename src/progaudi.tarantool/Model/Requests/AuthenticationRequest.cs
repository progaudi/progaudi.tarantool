using System;
using System.Buffers;
using System.Linq;
using ProGaudi.Buffers;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class AuthenticationRequest : Request, IDisposable
    {
        private readonly IMsgPackFormatter<AuthenticationRequest> _formatter;

        private AuthenticationRequest(string username, IMemoryOwner<byte> scramble, MsgPackContext context)
            : base(CommandCode.Auth, context)
        {
            Username = username;
            Scramble = scramble;
            _formatter = context.GetRequiredFormatter<AuthenticationRequest>();
        }

        public string Username { get; }

        public IMemoryOwner<byte> Scramble { get; }

        public static AuthenticationRequest Create(GreetingsResponse greetings, TarantoolNode node, MsgPackContext context)
        {
            var scramble = GetScramble(greetings, node.Pwd);
            return new AuthenticationRequest(node.User, scramble, context);
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

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}