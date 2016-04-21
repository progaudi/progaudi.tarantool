using System;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class JointRequestConverter:IMsgPackConverter<JoinRequestPacket>
    {
        public void Write(JoinRequestPacket value, IMsgPackWriter writer, MsgPackContext context)
        {
            var keyConverter = context.GetConverter<Key>();
            var codeConverter = context.GetConverter<CommandCode>();
            var intConverter = context.GetConverter<int>();
            var stringConverter = context.GetConverter<string>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.Code, writer, context);
            codeConverter.Write(CommandCode.Join, writer, context);

            keyConverter.Write(Key.Sync, writer, context);
            intConverter.Write(value.Sync, writer, context);

            keyConverter.Write(Key.ServerUuid, writer, context);
            stringConverter.Write(value.ServerUuid, writer, context);
        }

        public JoinRequestPacket Read(IMsgPackReader reader, MsgPackContext context, Func<JoinRequestPacket> creator)
        {
            throw new NotImplementedException();
        }
    }
}