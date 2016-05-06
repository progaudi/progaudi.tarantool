using System;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class SubscribePacketConverter : IMsgPackConverter<SubscribePacket>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(SubscribePacket value, IMsgPackWriter writer)
        {
            var keyConverter = _context.GetConverter<Key>();
            var codeConverter = _context.GetConverter<CommandCode>();
            var intConverter = _context.GetConverter<int>();
            var stringConverter = _context.GetConverter<string>();

            writer.WriteMapHeader(4);

            keyConverter.Write(Key.Code, writer);
            codeConverter.Write(CommandCode.Subscribe, writer);

            keyConverter.Write(Key.Sync, writer);
            intConverter.Write(value.Sync, writer);

            keyConverter.Write(Key.ServerUuid, writer);
            stringConverter.Write(value.ServerUuid, writer);

            keyConverter.Write(Key.ClusterUuid, writer);
            stringConverter.Write(value.ClusterUid, writer);

            writer.WriteMapHeader(1);

            keyConverter.Write(Key.Vclock, writer);
            intConverter.Write(value.Vclock, writer);
        }

        public SubscribePacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}