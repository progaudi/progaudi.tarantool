using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
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

        public sealed class Formatter : IMessagePackFormatter<DataResponse>
        {
            public int Serialize(ref byte[] bytes, int offset, DataResponse value, IFormatterResolver formatterResolver)
            {
                throw new System.NotImplementedException();
            }

            public DataResponse Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                if (MessagePackBinary.IsNil(bytes, offset))
                {
                    readSize = 1;
                    return null;
                }

                var startOffset = offset;
                var length = MessagePackBinary.ReadMapHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                var result = default(DataResponse);
                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
                    switch (key)
                    {
                        case Keys.SqlInfo:
                            result = new DataResponse(formatterResolver.GetFormatter<SqlInfo>().Deserialize(bytes, offset, formatterResolver, out readSize));
                            offset += readSize;
                            break;
                        default:
                            offset += MessagePackBinary.ReadNext(bytes, offset);
                            break;
                    }
                }

                readSize = offset - startOffset;

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

        public new class Formatter : IMessagePackFormatter<DataResponse<T>>
        {
            public int Serialize(ref byte[] bytes, int offset, DataResponse<T> value, IFormatterResolver formatterResolver)
            {
                throw new System.NotImplementedException();
            }

            public DataResponse<T> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                if (MessagePackBinary.IsNil(bytes, offset))
                {
                    readSize = 1;
                    return null;
                }

                var startOffset = offset;
                var length = MessagePackBinary.ReadMapHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                var data = default(T);
                var dataWasSet = false;
                var metadata = default(FieldMetadata[]);
                var sqlInfo = default(SqlInfo);

                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
                    switch (key)
                    {
                        case Keys.Data:
                            data = formatterResolver.GetFormatter<T>().Deserialize(bytes, offset, formatterResolver, out readSize);
                            dataWasSet = true;
                            offset += readSize;
                            break;
                        case Keys.Metadata:
                            metadata = formatterResolver.GetFormatter<FieldMetadata[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                            offset += readSize;
                            break;
                        case Keys.SqlInfo:
                            sqlInfo = formatterResolver.GetFormatter<SqlInfo>().Deserialize(bytes, offset, formatterResolver, out readSize);
                            offset += readSize;
                            break;
                        default:
                            offset += MessagePackBinary.ReadNext(bytes, offset);
                            break;
                    }
                }

                if (!dataWasSet && sqlInfo == null)
                {
                    throw ExceptionHelper.NoDataInDataResponse();
                }

                readSize = offset - startOffset;

                return new DataResponse<T>(data, metadata, sqlInfo);
            }
        }
    }
}