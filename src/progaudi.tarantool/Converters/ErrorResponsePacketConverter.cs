using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{

    internal class ErrorResponsePacketConverter : IMsgPackConverter<ErrorResponse>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
        }

        public void Write(ErrorResponse value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ErrorResponse Read(IMsgPackReader reader)
        {
            string errorMessage = null;
            var length = reader.ReadMapLength();
            
            var errorKey = _keyConverter.Read(reader);
            if (errorKey != Key.Error)
            {
                throw ExceptionHelper.UnexpectedKey(errorKey, Key.Error);
            }
            errorMessage = _stringConverter.Read(reader);
            
            return new ErrorResponse(errorMessage);
        }
    }
}