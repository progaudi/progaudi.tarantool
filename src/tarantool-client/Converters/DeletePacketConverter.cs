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
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(DeletePacket<T> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var keyConverter = _context.GetConverter<Key>();
            var uintConverter = _context.GetConverter<uint>();
            var selectKeyConverter = _context.GetConverter<T>();

            writer.WriteMapHeader(3);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.IndexId, writer);
            uintConverter.Write(value.IndexId, writer);

            keyConverter.Write(Key.Key, writer);
            selectKeyConverter.Write(value.Key, writer);
        }

        public DeletePacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}