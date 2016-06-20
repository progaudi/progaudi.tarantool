using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class InsertReplacePacketConverter<T> : IMsgPackConverter<InsertReplacePacket<T>>
        where T : ITuple
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(InsertReplacePacket<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(2);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);
        }

        public InsertReplacePacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}