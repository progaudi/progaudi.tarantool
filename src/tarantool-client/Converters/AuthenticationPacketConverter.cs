using System;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class AuthenticationPacketConverter : IMsgPackConverter<AuthenticationPacket>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(AuthenticationPacket value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var keyConverter = _context.GetConverter<Key>();
            var stringConverter = _context.GetConverter<string>();
            var bytesConverter = _context.GetConverter<byte[]>();

            writer.WriteMapHeader(2);

            keyConverter.Write(Key.Username, writer);
            stringConverter.Write(value.Username, writer);

            keyConverter.Write(Key.Tuple, writer);

            writer.WriteArrayHeader(2);
            stringConverter.Write("chap-sha1", writer);
            bytesConverter.Write(value.Scramble, writer);
        }

        public AuthenticationPacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}