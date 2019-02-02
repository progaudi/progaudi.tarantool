using System.Buffers;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ErrorResponsePacketParser : IMsgPackSequenceParser<ErrorResponse>
    {
        private readonly IMsgPackSequenceParser<Key> _keyParser;
        private readonly IMsgPackSequenceParser<string> _stringParser;

        public ErrorResponsePacketParser(MsgPackContext context)
        {
            _keyParser = context.GetRequiredSequenceParser<Key>();
            _stringParser = context.GetRequiredSequenceParser<string>();
        }

        public ErrorResponse Parse(ReadOnlySequence<byte> source, out int readSize)
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