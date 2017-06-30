using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class InsertReplacePacketConverter<T> : IMsgPackConverter<InsertReplaceRequest<T>>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _tupleConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _tupleConverter = context.GetConverter<T>();
        }

        public void Write(InsertReplaceRequest<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(2);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);
        }

        public InsertReplaceRequest<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}