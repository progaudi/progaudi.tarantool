using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class ResponsePacketConverter<T> : IMsgPackConverter<ResponsePacket<T>>
    {
        private IMsgPackConverter<Key> _keyConverter;

        private IMsgPackConverter<T> _dataConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _dataConverter = context.GetConverter<T>();
        }

        public void Write(ResponsePacket<T> value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket<T> Read(IMsgPackReader reader)
        {
            var data = default(T);

            var length = reader.ReadMapLength();

            if (!length.HasValue)
            {
                throw new ArgumentException("Map length should not be null!");
            }

            if (length.Value <= 0u)
            {
                return new ResponsePacket<T>(data);
            }

            var dataKey = _keyConverter.Read(reader);
            if (dataKey != Key.Data)
            {
                throw ExceptionHelper.UnexpectedKey(Key.Data, dataKey);
            }

            data = _dataConverter.Read(reader);

            return new ResponsePacket<T>(data);
        }
    }
}