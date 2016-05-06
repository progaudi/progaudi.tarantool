using System;
using System.Collections.Generic;

using iproto.Data;
using iproto.Data.Packets;

using Shouldly;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class JoinResponsePacketConverter : IMsgPackConverter<JoinResponsePacket>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(JoinResponsePacket value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public JoinResponsePacket Read(IMsgPackReader reader)
        {
            var keyConverter = _context.GetConverter<Key>();
            var codeConverter = _context.GetConverter<CommandCode>();
            var intConverter = _context.GetConverter<int>();
            var vclockConverter = _context.GetConverter<Dictionary<int, int>>();

            reader.ReadMapLength().ShouldBe(2u);

            keyConverter.Read(reader).ShouldBe(Key.Code);
            codeConverter.Read(reader).ShouldBe(CommandCode.Ok);

            keyConverter.Read(reader).ShouldBe(Key.Sync);
            var sync = intConverter.Read(reader);

            reader.ReadMapLength().ShouldBe(1u);

            keyConverter.Read(reader).ShouldBe(Key.Vclock);
            var vclocks = vclockConverter.Read(reader);

            return new JoinResponsePacket(sync, vclocks);
        }
    }
}