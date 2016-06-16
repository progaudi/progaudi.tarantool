using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class DeletePacketConverter<T> : IMsgPackConverter<DeletePacket<T>>
        where T: ITuple
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _selectKeyConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _selectKeyConverter = context.GetConverter<T>();
        }

        public void Write(DeletePacket<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.IndexId, writer);
            _uintConverter.Write(value.IndexId, writer);

            _keyConverter.Write(Key.Key, writer);
            _selectKeyConverter.Write(value.Key, writer);
        }

        public DeletePacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}