using System;
using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public class SelectRequest<T> : IRequest
    {
        public SelectRequest(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, T selectKey)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public T SelectKey { get; }

        public CommandCodes Code => CommandCodes.Select;

        public sealed class Formatter : IMsgPackConverter<SelectRequest<T>>
        {
            private IMsgPackConverter<T> _selectKeyConverter;
            private IMsgPackConverter<uint> _uintConverter;
            private IMsgPackConverter<Iterator> _iteratorConverter;

            public void Initialize(MsgPackContext context)
            {
                _uintConverter = context.GetConverter<uint>();
                _iteratorConverter = context.GetConverter<Iterator>();
                _selectKeyConverter = context.GetConverter<T>();
            }

            public void Write(SelectRequest<T> value, IMsgPackWriter writer)
            {
                writer.WriteMapHeader(6);

                _uintConverter.Write(Keys.SpaceId, writer);
                _uintConverter.Write(value.SpaceId, writer);

                _uintConverter.Write(Keys.IndexId, writer);
                _uintConverter.Write(value.IndexId, writer);

                _uintConverter.Write(Keys.Limit, writer);
                _uintConverter.Write(value.Limit, writer);

                _uintConverter.Write(Keys.Offset, writer);
                _uintConverter.Write(value.Offset, writer);

                _uintConverter.Write(Keys.Iterator, writer);
                _iteratorConverter.Write(value.Iterator, writer);

                _uintConverter.Write(Keys.Key, writer);
                _selectKeyConverter.Write(value.SelectKey, writer);
            }

            public SelectRequest<T> Read(IMsgPackReader reader)
            {
                throw new NotSupportedException();
            }
        }
    }
}