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
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(InsertReplacePacket<T> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var keyConverter = _context.GetConverter<Key>();
            var uintConverter = _context.GetConverter<uint>();
            var tupleConverter = _context.GetConverter<T>();

            writer.WriteMapHeader(2);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.Tuple, writer);
            tupleConverter.Write(value.Tuple, writer);
        }

        public InsertReplacePacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}