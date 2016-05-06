using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class InsertReplacePacketConverter<T> : IMsgPackConverter<InsertReplacePacket<T>>
        where T : ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(InsertReplacePacket<T> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);

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