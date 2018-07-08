using System;
using System.Collections.Generic;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
{
    public class DataResponse
    {
        public DataResponse(SqlInfo sqlInfo)
        {
            SqlInfo = sqlInfo;
        }

        public SqlInfo SqlInfo { get; }

        public sealed class Formatter : IMsgPackConverter<DataResponse>
        {
            private IMsgPackConverter<uint> _keyConverter;
            private IMsgPackConverter<int> _intConverter;

            public void Initialize(MsgPackContext context)
            {
                _keyConverter = context.GetConverter<uint>();
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
                        case Keys.SqlInfo:
                            sqlInfo = ReadSqlInfo(reader, _keyConverter, _intConverter);
                            break;
                        default:
                            throw ExceptionHelper.UnexpectedKey(dataKey, Keys.SqlInfo);
                    }
                }

                return new DataResponse(sqlInfo);
            }

            internal static SqlInfo ReadSqlInfo(IMsgPackReader reader, IMsgPackConverter<uint> keyConverter, IMsgPackConverter<int> intConverter)
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
                        case Keys.SqlRowCount:
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
    }

    public class DataResponse<T> : DataResponse
    {
        public DataResponse(T data, FieldMetadata[] metadata, SqlInfo sqlInfo)
            : base(sqlInfo)
        {
            Data = data;
            MetaData = metadata;
        }

        public T Data { get; }

        public IReadOnlyList<FieldMetadata> MetaData { get; }

        public new class Formatter : IMsgPackConverter<DataResponse<T>>
        {
            private IMsgPackConverter<uint> _keyConverter;
            private IMsgPackConverter<T> _dataConverter;
            private IMsgPackConverter<string> _stringConverter;
            private IMsgPackConverter<int> _intConverter;

            public void Initialize(MsgPackContext context)
            {
                _keyConverter = context.GetConverter<uint>();
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
                        case Keys.Data:
                            data = _dataConverter.Read(reader);
                            dataWasSet = true;
                            break;
                        case Keys.Metadata:
                            metadata = ReadMetadata(reader);
                            break;
                        case Keys.SqlInfo:
                            sqlInfo = DataResponse.Formatter.ReadSqlInfo(reader, _keyConverter, _intConverter);
                            break;
                        default:
                            throw ExceptionHelper.UnexpectedKey(dataKey, Keys.Data, Keys.Metadata, Keys.SqlInfo);
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
                            case Keys.FieldName:
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
}