using System;

using ProGaudi.MsgPack;

using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class AuthenticationPacketFormatter : IMsgPackFormatter<AuthenticationRequest>
    {
        private static readonly ReadOnlyMemory<byte> ChapSha1;
        private readonly IMsgPackFormatter<Key> _keyFormatter;
        private readonly IMsgPackFormatter<ReadOnlyMemory<byte>> _bytesFormatter;
        private readonly IMsgPackFormatter<string> _stringFormatter;

        public AuthenticationPacketFormatter(MsgPackContext context)
        {
            _keyFormatter = context.GetRequiredFormatter<Key>();
            _bytesFormatter = context.GetRequiredFormatter<ReadOnlyMemory<byte>>();
            _stringFormatter = context.GetRequiredFormatter<string>();
        }

        static AuthenticationPacketFormatter()
        {
            var memory = new byte["chap-sha1".Length + 1];
            MsgPackSpec.WriteFixString(memory, "chap-sha1");
            ChapSha1 = memory;
        }

        public int GetBufferSize(AuthenticationRequest value) => value == null ? DataLengths.Nil 
            : DataLengths.FixMapHeader
            + 2 * DataLengths.UInt32
            + _stringFormatter.GetBufferSize(value.Username)
            + DataLengths.FixArrayHeader
            + ChapSha1.Length
            + _bytesFormatter.GetBufferSize(value.Scramble.Memory);

        public bool HasConstantSize => false;
        
        public int Format(Span<byte> destination, AuthenticationRequest value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);
            result += _keyFormatter.Format(destination.Slice(result), Key.Username);
            result += _stringFormatter.Format(destination.Slice(result), value.Username);
            
            result += _keyFormatter.Format(destination.Slice(result), Key.Tuple);
            
            result += MsgPackSpec.WriteFixArrayHeader(destination.Slice(result), 2);
            ChapSha1.Span.CopyTo(destination.Slice(result));
            result += ChapSha1.Length;
            result += _bytesFormatter.GetBufferSize(value.Scramble.Memory);
            return result;
        }
    }
}