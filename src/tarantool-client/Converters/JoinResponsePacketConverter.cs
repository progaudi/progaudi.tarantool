using System;
using System.Collections.Generic;

using iproto.Data;
using iproto.Data.Packets;

using Shouldly;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class JoinResponsePacketConverter :IMsgPackConverter<JoinResponsePacket>
    {
        public void Write(JoinResponsePacket value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new NotImplementedException();
        }

        public JoinResponsePacket Read(IMsgPackReader reader, MsgPackContext context, Func<JoinResponsePacket> creator)
        {
            var keyConverter = context.GetConverter<Key>();
            var codeConverter = context.GetConverter<CommandCode>();
            var intConverter = context.GetConverter<int>();
            var vclockConverter = context.GetConverter<Dictionary<int, int>>();

            reader.ReadMapLength().ShouldBe(2u);

            keyConverter.Read(reader, context, null).ShouldBe(Key.Code);
            codeConverter.Read(reader, context, null).ShouldBe(CommandCode.Ok);

            keyConverter.Read(reader, context, null).ShouldBe(Key.Sync);
            var sync = intConverter.Read(reader, context, null);

            reader.ReadMapLength().ShouldBe(1u);

            keyConverter.Read(reader, context, null).ShouldBe(Key.Vclock);
            var vclocks = vclockConverter.Read(reader, context, null);

            return new JoinResponsePacket(sync, vclocks);
        }
    }
}