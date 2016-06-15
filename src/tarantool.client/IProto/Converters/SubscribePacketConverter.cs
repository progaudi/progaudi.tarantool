using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class SubscribePacketConverter : IMsgPackConverter<SubscribePacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;
        private IMsgPackConverter<int> _intConverter;
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _codeConverter = context.GetConverter<CommandCode>();
            _intConverter = context.GetConverter<int>();
            _stringConverter = context.GetConverter<string>();

        }

        public void Write(SubscribePacket value, IMsgPackWriter writer)
        {

            writer.WriteMapHeader(4);

            _keyConverter.Write(Key.Code, writer);
            _codeConverter.Write(CommandCode.Subscribe, writer);

            _keyConverter.Write(Key.Sync, writer);
            _intConverter.Write(value.Sync, writer);

            _keyConverter.Write(Key.ServerUuid, writer);
            _stringConverter.Write(value.ServerUuid, writer);

            _keyConverter.Write(Key.ClusterUuid, writer);
            _stringConverter.Write(value.ClusterUid, writer);

            writer.WriteMapHeader(1);

            _keyConverter.Write(Key.Vclock, writer);
            _intConverter.Write(value.Vclock, writer);
        }

        public SubscribePacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}