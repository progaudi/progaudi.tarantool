using System;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class JointRequestConverter : IMsgPackConverter<JoinRequestPacket>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(JoinRequestPacket value, IMsgPackWriter writer)
        {
            var keyConverter = _context.GetConverter<Key>();
            var codeConverter = _context.GetConverter<CommandCode>();
            var intConverter = _context.GetConverter<int>();
            var stringConverter = _context.GetConverter<string>();

            writer.WriteMapHeader(3);

            keyConverter.Write(Key.Code, writer);
            codeConverter.Write(CommandCode.Join, writer);

            keyConverter.Write(Key.Sync, writer);
            intConverter.Write(value.Sync, writer);

            keyConverter.Write(Key.ServerUuid, writer);
            stringConverter.Write(value.ServerUuid, writer);
        }

        public JoinRequestPacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}