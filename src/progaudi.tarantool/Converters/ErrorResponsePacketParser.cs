using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ErrorResponsePacketParser : IMsgPackParser<ErrorResponse>
    {
        private readonly IMsgPackParser<Key> _keyParser;
        private readonly IMsgPackParser<string> _stringParser;

        public ErrorResponsePacketParser(MsgPackContext context)
        {
            _keyParser = context.GetRequiredParser<Key>();
            _stringParser = context.GetRequiredParser<string>();
        }

        public ErrorResponse Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);

            if (length != 1u)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u);
            }

            var errorKey = _keyParser.Parse(source.Slice(readSize), out var temp);
            readSize += temp;
            if (errorKey != Key.Error)
            {
                throw ExceptionHelper.UnexpectedKey(errorKey, Key.Error);
            }

            var errorMessage = _stringParser.Parse(source.Slice(readSize), out temp);
            readSize += temp;
            return new ErrorResponse(errorMessage);
        }
    }
}