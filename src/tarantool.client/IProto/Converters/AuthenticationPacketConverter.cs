using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class AuthenticationPacketConverter : IMsgPackConverter<AuthenticationPacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<byte[]> _bytesConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _bytesConverter = context.GetConverter<byte[]>();
            _stringConverter = context.GetConverter<string>();
            _nullConverter = context.NullConverter;
        }

        public void Write(AuthenticationPacket value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _nullConverter.Write(null, writer);
                return;
            }

            writer.WriteMapHeader(2);

            _keyConverter.Write(Key.Username, writer);
            _stringConverter.Write(value.Username, writer);

            _keyConverter.Write(Key.Tuple, writer);

            writer.WriteArrayHeader(2);
            _stringConverter.Write("chap-sha1", writer);
            _bytesConverter.Write(value.Scramble, writer);
        }

        public AuthenticationPacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}