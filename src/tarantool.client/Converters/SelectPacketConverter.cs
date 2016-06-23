using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;

namespace Tarantool.Client.Converters
{
    internal class SelectPacketConverter<T> : IMsgPackConverter<SelectRequest<T>>
        where T : ITuple
    {
        private IMsgPackConverter<T> _selectKeyConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Iterator> _iteratorConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _uintConverter = context.GetConverter<uint>();
            _iteratorConverter = context.GetConverter<Iterator>();
            _selectKeyConverter = context.GetConverter<T>();
        }

        public void Write(SelectRequest<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(6);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.IndexId, writer);
            _uintConverter.Write(value.IndexId, writer);

            _keyConverter.Write(Key.Limit, writer);
            _uintConverter.Write(value.Limit, writer);

            _keyConverter.Write(Key.Offset, writer);
            _uintConverter.Write(value.Offset, writer);

            _keyConverter.Write(Key.Iterator, writer);
            _iteratorConverter.Write(value.Iterator, writer);

            _keyConverter.Write(Key.Key, writer);
            _selectKeyConverter.Write(value.SelectKey, writer);
        }

        public SelectRequest<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}