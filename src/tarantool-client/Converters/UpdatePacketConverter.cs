using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpdatePacketConverter<T, TUpdate> : IMsgPackConverter<UpdatePacket<T, TUpdate>>
        where T : ITuple
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(UpdatePacket<T, TUpdate> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var uintConverter = _context.GetConverter<uint>();
            var keyConverter = _context.GetConverter<Key>();
            var selectKeyConverter = _context.GetConverter<T>();
            var updateOperationConverter = _context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeader(4);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.IndexId, writer);
            uintConverter.Write(value.IndexId, writer);

            keyConverter.Write(Key.Key, writer);
            selectKeyConverter.Write(value.Key, writer);

            keyConverter.Write(Key.Tuple, writer);
            writer.WriteArrayHeader(1);
            updateOperationConverter.Write(value.UpdateOperation, writer);
        }

        public UpdatePacket<T, TUpdate> Read(IMsgPackReader readerr)
        {
            throw new NotImplementedException();
        }
    }
}