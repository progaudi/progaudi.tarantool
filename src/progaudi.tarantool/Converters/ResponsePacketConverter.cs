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
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _dataConverter = context.GetConverter<T>();
            _stringConverter = context.GetConverter<string>();
        }

        public void Write(DataResponse<T> value, IMsgPackWriter writer)
        {
            throw new NotSupportedException();
        }

        public DataResponse<T> Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length != 1u && length != 2u)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var data = default(T);
            var dataWasSet = false;
            var metadata = default(FieldMetadata[]);
            for (var i = 0; i < length; i++)
            {
                var dataKey = _keyConverter.Read(reader);
                switch (dataKey)
                {
                    case Key.Data:
                        data = _dataConverter.Read(reader);
                        dataWasSet = true;
                        break;
                    case Key.Metadata:
                        metadata = ReadMetadata(reader);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            if (!dataWasSet)
            {
                throw ExceptionHelper.NoDataInDataResponse();
            }

            return new DataResponse<T>(data, metadata);
        }

        private FieldMetadata[] ReadMetadata(IMsgPackReader reader)
        {
            var length = reader.ReadArrayLength();
            if (length == null)
            {
                return null;
            }

            var result = new FieldMetadata[length.Value];
            for (var i = 0; i < length; i++)
            {
                var metadataLength = reader.ReadMapLength();
                if (metadataLength == null)
                {
                    result[i] = null;
                    continue;
                }

                for (var j = 0; j < metadataLength; j++)
                {
                    switch (_keyConverter.Read(reader))
                    {
                        case Key.FieldName:
                            result[i] = new FieldMetadata(_stringConverter.Read(reader));
                            continue;
                        default:
                            reader.SkipToken();
                            break;
                    }
                }
            }

            return result;
        }
    }
}