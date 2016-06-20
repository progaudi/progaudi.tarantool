using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class CallPacketConverter<T> : IMsgPackConverter<CallPacket<T>>
        where T : ITuple
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(CallPacket<T> value, IMsgPackWriter writer)
        {
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