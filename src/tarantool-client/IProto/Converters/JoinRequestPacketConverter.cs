using System;

using MsgPack.Light;

using tarantool_client.IProto.Data;
using tarantool_client.IProto.Data.Packets;

namespace tarantool_client.IProto.Converters
{
    public class JointRequestConverter : IMsgPackConverter<JoinRequestPacket>
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

        public void Write(JoinRequestPacket value, IMsgPackWriter writer)
        {
    
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.Code, writer);
            _codeConverter.Write(CommandCode.Join, writer);

            _keyConverter.Write(Key.Sync, writer);
            _intConverter.Write(value.Sync, writer);

            _keyConverter.Write(Key.ServerUuid, writer);
            _stringConverter.Write(value.ServerUuid, writer);
        }

        public JoinRequestPacket Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}