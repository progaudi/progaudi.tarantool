using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class ErrorResponsePacketConverter : IMsgPackConverter<ErrorResponsePacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
        }

        public void Write(ErrorResponsePacket value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ErrorResponsePacket Read(IMsgPackReader reader)
        {
            string errorMessage = null;
            var length = reader.ReadMapLength();

            if (length != 1u)
            {
                throw ExceptionHelper.InvalidMapLength(1u, length);
            }

            var errorKey = _keyConverter.Read(reader);
            if (errorKey != Key.Error)
            {
                throw ExceptionHelper.UnexpectedKey(Key.Error, errorKey);
            }

            errorMessage = _stringConverter.Read(reader);

            return new ErrorResponsePacket(errorMessage);
        }
    }
}