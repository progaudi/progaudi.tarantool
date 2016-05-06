using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class SelectPacketConverter<T> : IMsgPackConverter<SelectPacket<T>>
        where T : ITuple
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(SelectPacket<T> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var keyConverter = _context.GetConverter<Key>();
            var uintConverter = _context.GetConverter<uint>();
            var iteratorConverter = _context.GetConverter<Iterator>();
            var selectKeyConverter = _context.GetConverter<T>();

            writer.WriteMapHeader(6);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.IndexId, writer);
            uintConverter.Write(value.IndexId, writer);

            keyConverter.Write(Key.Limit, writer);
            uintConverter.Write(value.Limit, writer);

            keyConverter.Write(Key.Offset, writer);
            uintConverter.Write(value.Offset, writer);

            keyConverter.Write(Key.Iterator, writer);
            iteratorConverter.Write(value.Iterator, writer);

            keyConverter.Write(Key.Key, writer);
            selectKeyConverter.Write(value.SelectKey, writer);
        }

        public SelectPacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}