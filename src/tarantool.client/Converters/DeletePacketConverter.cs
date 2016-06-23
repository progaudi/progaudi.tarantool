using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;

namespace Tarantool.Client.Converters
{
    internal class DeletePacketConverter<T> : IMsgPackConverter<DeleteRequest<T>>
        where T: ITuple
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<T> _selectKeyConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _selectKeyConverter = context.GetConverter<T>();
        }

        public void Write(DeleteRequest<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.IndexId, writer);
            _uintConverter.Write(value.IndexId, writer);

            _keyConverter.Write(Key.Key, writer);
            _selectKeyConverter.Write(value.Key, writer);
        }

        public DeleteRequest<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}