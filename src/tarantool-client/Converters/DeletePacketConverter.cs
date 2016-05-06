using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class DeletePacketConverter<T> : IMsgPackConverter<DeletePacket<T>>
        where T: ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _selectKeyConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _selectKeyConverter = context.GetConverter<T>();
        }

        public void Write(DeletePacket<T> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);

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