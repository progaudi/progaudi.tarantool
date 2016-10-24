using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class EmptyResponseConverter : IMsgPackConverter<EmptyResponse>
    {
        private IMsgPackConverter<Key> _keyConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
        }

        public void Write(EmptyResponse value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public EmptyResponse Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length > 1)
            {
                throw ExceptionHelper.InvalidMapLength(length, 0, 1);
            }

            if (length ==1)
            {
                var dataKey = _keyConverter.Read(reader);
                if (dataKey != Key.Data)
                {
                    throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data);
                }

                var arrayLength = reader.ReadArrayLength();
                if (arrayLength != 0)
                {
                    throw ExceptionHelper.InvalidArrayLength(0, length);
                }
            }

            return new EmptyResponse();

        }
    }
}