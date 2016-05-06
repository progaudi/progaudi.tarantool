using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class EvalPacketConverter<T> : IMsgPackConverter<EvalPacket<T>>
        where T:ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(EvalPacket<T> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);


            writer.WriteMapHeader(2);

            _keyConverter.Write(Key.Expression, writer);
            _stringConverter.Write(value.Expression, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);
        }

        public EvalPacket<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}