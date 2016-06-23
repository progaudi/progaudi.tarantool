using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;

namespace Tarantool.Client.Converters
{
    internal class InsertReplacePacketConverter<T> : IMsgPackConverter<InsertReplaceRequest<T>>
        where T : ITuple
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