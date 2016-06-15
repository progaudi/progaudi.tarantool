using System;
using System.Collections.Generic;

using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class JoinResponsePacketConverter : IMsgPackConverter<JoinResponsePacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;
        private IMsgPackConverter<int> _intConverter;
        private IMsgPackConverter<Dictionary<int, int>> _vclockConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _codeConverter = context.GetConverter<CommandCode>();
            _intConverter = context.GetConverter<int>();
            _vclockConverter = context.GetConverter<Dictionary<int, int>>();
        }

        public void Write(JoinResponsePacket value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public JoinResponsePacket Read(IMsgPackReader reader)
        {
        
            reader.ReadMapLength().ShouldBe(2u);

            _keyConverter.Read(reader).ShouldBe(Key.Code);
            _codeConverter.Read(reader).ShouldBe(CommandCode.Ok);

            _keyConverter.Read(reader).ShouldBe(Key.Sync);
            var sync = _intConverter.Read(reader);

            reader.ReadMapLength().ShouldBe(1u);

            _keyConverter.Read(reader).ShouldBe(Key.Vclock);
            var vclocks = _vclockConverter.Read(reader);

            return new JoinResponsePacket(sync, vclocks);
        }
    }
}