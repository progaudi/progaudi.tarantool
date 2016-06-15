using System;

using MsgPack.Light;

using tarantool_client.IProto.Data;
using tarantool_client.IProto.Data.Packets;

namespace tarantool_client.IProto.Converters
{
    public class CallPacketConverter<T> : IMsgPackConverter<CallPacket<T>>
        where T : ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(CallPacket<T> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);

            writer.WriteMapHeader(2);

            _keyConverter.Write(Key.FunctionName, writer);
            _stringConverter.Write(value.FunctionName, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);
        }

        public CallPacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}