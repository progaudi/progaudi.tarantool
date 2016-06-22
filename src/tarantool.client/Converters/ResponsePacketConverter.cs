using System;

using MsgPack.Light;

using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    public class ResponsePacketConverter<T> : IMsgPackConverter<DataResponse<T>>
    {
        private IMsgPackConverter<Key> _keyConverter;

        private IMsgPackConverter<T> _dataConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _dataConverter = context.GetConverter<T>();
        }

        public void Write(DataResponse<T> value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public DataResponse<T> Read(IMsgPackReader reader)
        {
            var data = default(T);

            var length = reader.ReadMapLength();

            if (!length.HasValue)
            {
                throw new ArgumentException("Map length should not be null!");
            }

            if (length.Value <= 0u)
            {
                return new DataResponse<T>(data);
            }

            var dataKey = _keyConverter.Read(reader);
            if (dataKey != Key.Data)
            {
                throw ExceptionHelper.UnexpectedKey(Key.Data, dataKey);
            }

            data = _dataConverter.Read(reader);

            return new DataResponse<T>(data);
        }
    }
}