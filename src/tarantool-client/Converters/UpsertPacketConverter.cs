using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpsertPacketConverter<T, TUpdate> : IMsgPackConverter<UpsertPacket<T, TUpdate>>
        where T : ITuple
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(UpsertPacket<T, TUpdate> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var uintConverter = _context.GetConverter<uint>();
            var keyConverter = _context.GetConverter<Key>();
            var tupleConverter = _context.GetConverter<T>();
            var updateOperationConverter = _context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeader(3);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.Tuple, writer);
            tupleConverter.Write(value.Tuple, writer);

            keyConverter.Write(Key.Ops, writer);
            writer.WriteArrayHeader(1);
            updateOperationConverter.Write(value.UpdateOperation, writer);
        }

        public UpsertPacket<T, TUpdate> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}