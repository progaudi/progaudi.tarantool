using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class CallPacketConverter<T> : IMsgPackConverter<CallPacket<T>>
        where T : ITuple
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(CallPacket<T> value, IMsgPackWriter writer)
        {
            var headerConverter = _context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer);

            var keyConverter = _context.GetConverter<Key>();
            var stringConverter = _context.GetConverter<string>();
            var tupleConverter = _context.GetConverter<T>();

            writer.WriteMapHeader(2);

            keyConverter.Write(Key.FunctionName, writer);
            stringConverter.Write(value.FunctionName, writer);

            keyConverter.Write(Key.Tuple, writer);
            tupleConverter.Write(value.Tuple, writer);
        }

        public CallPacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}