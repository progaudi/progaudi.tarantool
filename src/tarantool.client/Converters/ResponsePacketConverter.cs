using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ResponsePacketConverter<T> : IMsgPackConverter<DataResponse<T>>
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
            if (length != 1u)
            {
                throw ExceptionHelper.InvalidMapLength(length, 3u);
            }

            var dataKey = _keyConverter.Read(reader);
            if (dataKey != Key.Data)
            {
                throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data);
            }

            data = _dataConverter.Read(reader);

            return new DataResponse<T>(data);
        }
    }
}