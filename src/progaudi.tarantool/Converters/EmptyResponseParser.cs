using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class EmptyResponseParser : IMsgPackParser<EmptyResponse>
    {
        private readonly IMsgPackParser<Key> _keyParser;

        public EmptyResponseParser(MsgPackContext context)
        {
            _keyParser = context.GetRequiredParser<Key>();
        }

        public EmptyResponse Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);

            if (length > 1) throw ExceptionHelper.InvalidMapLength(length, 0, 1);
            if (length != 1) return new EmptyResponse();

            var dataKey = _keyParser.Parse(source.Slice(readSize), out var temp);
            readSize += temp;
            if (dataKey != Key.Data)
            {
                throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data);
            }

            var arrayLength = MsgPackSpec.ReadArrayHeader(source, out temp);
            readSize += temp;
            if (arrayLength != 0)
            {
                throw ExceptionHelper.InvalidArrayLength(0u, arrayLength);
            }

            return new EmptyResponse();
        }
    }
}