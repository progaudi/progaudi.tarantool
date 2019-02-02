using System.Buffers;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ResponsePacketParser : IMsgPackSequenceParser<DataResponse>
    {
        private readonly IMsgPackSequenceParser<Key> _keyConverter;

        public ResponsePacketParser(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredSequenceParser<Key>();
        }

        public DataResponse Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
            if (!(1u <= length && length <= 3))
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var sqlInfo = default(SqlInfo);
            for (var i = 0; i < length; i++)
            {
                var dataKey = _keyConverter.Parse(source, out var temp);
                readSize += temp;
                switch (dataKey)
                {
                    case Key.SqlInfo:
                        sqlInfo = ReadSqlInfo(source, _keyConverter, ref readSize);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            return new DataResponse(sqlInfo);
        }

        internal static SqlInfo ReadSqlInfo(ReadOnlySequence<byte> source, IMsgPackSequenceParser<Key> keyConverter, ref int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source.Slice(readSize), out var temp);
            readSize += temp;

            var result = default(SqlInfo);
            for (var i = 0; i < length; i++)
            {
                var dataKey = keyConverter.Parse(source, out temp);
                readSize += temp;
                switch (dataKey)
                {
                    case Key.SqlRowCount:
                        result = new SqlInfo(MsgPackSpec.ReadInt32(source.Slice(readSize), out temp));
                        readSize += temp;
                        break;
                    default:
                        readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).GetIntLength();
                        break;
                }
            }

            return result;
        }
    }

    internal class ResponsePacketParser<T> : IMsgPackSequenceParser<DataResponse<T>>
    {
        private readonly IMsgPackSequenceParser<Key> _keyConverter;
        private readonly IMsgPackSequenceParser<T> _dataConverter;

        public ResponsePacketParser(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredSequenceParser<Key>();
            _dataConverter = context.GetRequiredSequenceParser<T>();
        }

        public DataResponse<T> Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
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
                var dataKey = _keyConverter.Parse(source.Slice(readSize), out var temp); readSize += temp;
                switch (dataKey)
                {
                    case Key.Data:
                        data = _dataConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
                        dataWasSet = true;
                        break;
                    case Key.Metadata:
                        metadata = ReadMetadata(source, ref readSize);
                        break;
                    case Key.SqlInfo:
                        sqlInfo = ResponsePacketParser.ReadSqlInfo(source, _keyConverter, ref readSize);
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

        private FieldMetadata[] ReadMetadata(ReadOnlySequence<byte> source, ref int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out var temp); readSize += temp;
            var result = new FieldMetadata[length];

            for (var i = 0; i < length; i++)
            {
                var metadataLength = MsgPackSpec.TryReadNil(source, out temp)
                    ? default(uint?)
                    : MsgPackSpec.ReadMapHeader(source, out temp);
                readSize += temp;

                if (metadataLength == null)
                {
                    result[i] = null;
                    continue;
                }

                for (var j = 0; j < metadataLength; j++)
                {
                    var key = _keyConverter.Parse(source.Slice(readSize), out temp);
                    readSize += temp;
                    switch (key)
                    {
                        case Key.FieldName:
                            result[i] = new FieldMetadata(MsgPackSpec.ReadString(source.Slice(readSize), out temp));
                            readSize += temp;
                            continue;
                        default:
                            readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).GetIntLength();
                            break;
                    }
                }
            }

            return result;
        }
    }
}