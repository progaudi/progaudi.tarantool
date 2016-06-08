using System;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class AuthenticationPacketConverter : IMsgPackConverter<AuthenticationPacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<byte[]> _bytesConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _headerConverter = context.GetConverter<Header>();
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

            _headerConverter.Write(value.Header, writer);

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