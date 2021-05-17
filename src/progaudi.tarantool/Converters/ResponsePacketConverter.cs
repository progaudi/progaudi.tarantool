using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ResponsePacketConverter : IMsgPackConverter<DataResponse>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<int> _intConverter;

        public virtual void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _intConverter = context.GetConverter<int>();
        }

        public void Write(DataResponse value, IMsgPackWriter writer)
        {
            throw new NotSupportedException();
        }

        public DataResponse Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();
            if (!(1u <= length && length <= 3))
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var sqlInfo = default(SqlInfo);

            for (var i = 0; i < length; i++)
            {
                var dataKey = _keyConverter.Read(reader);
                switch (dataKey)
                {
                    case Key.SqlInfo:
                        sqlInfo = ReadSqlInfo(reader, _keyConverter, _intConverter);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = ReadSqlInfo(reader, _keyConverter, _intConverter);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            return new DataResponse(sqlInfo);
        }

        internal static SqlInfo ReadSqlInfo(IMsgPackReader reader, IMsgPackConverter<Key> keyConverter, IMsgPackConverter<int> intConverter)
        {
            var length = reader.ReadMapLength();
            if (length == null)
            {
                return null;
            }

            var result = default(SqlInfo);
            for (var i = 0; i < length; i++)
            {
                switch (keyConverter.Read(reader))
                {
                    case Key.SqlRowCount:
                        result = new SqlInfo(intConverter.Read(reader));
                        break;
                    case Key.SqlRowCount_2_0_4:
                        result = new SqlInfo(intConverter.Read(reader));
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            return result;
        }
    }

    internal class ResponsePacketConverter<T> : IMsgPackConverter<DataResponse<T>>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _dataConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<int> _intConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _dataConverter = context.GetConverter<T>();
            _stringConverter = context.GetConverter<string>();
            _intConverter = context.GetConverter<int>();
        }

        public void Write(DataResponse<T> value, IMsgPackWriter writer)
        {
            throw new NotSupportedException();
        }

        DataResponse<T> IMsgPackConverter<DataResponse<T>>.Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();
            if (!(1u <= length && length <= 3))
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var data = default(T);
            var dataWasSet = false;
            var metadata = default(FieldMetadata[]);
            var sqlInfo = default(SqlInfo);

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
                    case Key.SqlInfo:
                        sqlInfo = ResponsePacketConverter.ReadSqlInfo(reader, _keyConverter, _intConverter);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = ResponsePacketConverter.ReadSqlInfo(reader, _keyConverter, _intConverter);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            if (!dataWasSet && sqlInfo == null)
            {
                throw ExceptionHelper.NoDataInDataResponse();
            }

            return new DataResponse<T>(data, metadata, sqlInfo);
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
                        case Key.FieldName_2_0_4:
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