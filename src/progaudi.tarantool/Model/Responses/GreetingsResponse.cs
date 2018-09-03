using System;
using System.Text;

namespace ProGaudi.Tarantool.Client.Model.Responses
{
    public class GreetingsResponse
    {
        private static readonly Encoding _encoding = Encoding.ASCII;
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        public GreetingsResponse(ReadOnlySpan<byte> span)
        {
            if (span.Length != 128)
                throw new InvalidOperationException("Greetings packet length should be 128");

            Message = _encoding.GetString(span.Slice(0, GreetingsMessageLength));
            var saltString = _encoding.GetString(span.Slice(GreetingsMessageLength, GreetingsSaltLength));
            Salt = Convert.FromBase64String(saltString);
        }

        public string Message { get; }

        public byte[] Salt { get; }
    }
}