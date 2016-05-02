using System;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class SubscribePacketConverter : IMsgPackConverter<SubscribePacket>
    {
        public void Write(SubscribePacket value, IMsgPackWriter writer, MsgPackContext context)
        {
            var keyConverter = context.GetConverter<Key>();
            var codeConverter = context.GetConverter<CommandCode>();
            var intConverter = context.GetConverter<int>();
            var stringConverter = context.GetConverter<string>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.Code, writer, context);
            codeConverter.Write(CommandCode.Subscribe, writer, context);

            keyConverter.Write(Key.Sync, writer, context);
            intConverter.Write(value.Sync, writer, context);

            keyConverter.Write(Key.ServerUuid, writer, context);
            stringConverter.Write(value.ServerUuid, writer, context);

            keyConverter.Write(Key.ClusterUuid, writer, context);
            stringConverter.Write(value.ClusterUid, writer, context);

            writer.WriteMapHeaderAndLength(1);

            keyConverter.Write(Key.Vclock, writer, context);
            intConverter.Write(value.Vclock, writer, context);
        }

        public SubscribePacket Read(IMsgPackReader reader, MsgPackContext context, Func<SubscribePacket> creator)
        {
            throw new NotImplementedException();
        }
    }
}