using System;

using MsgPack.Light;

using Shouldly;

using tarantool_client.IProto.Data;
using tarantool_client.IProto.Data.Packets;

namespace tarantool_client.IProto.Converters
{
    public class ResponsePacketConverter<T> : IMsgPackConverter<ResponsePacket<T>>
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<T> _dataConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
            _dataConverter = context.GetConverter<T>();
        }

        public void Write(ResponsePacket<T> value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket<T> Read(IMsgPackReader reader)
        {
            var header = _headerConverter.Read(reader);
            string errorMessage = null;
            var data = default(T);

            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                length.ShouldBe(1u);

                _keyConverter.Read(reader).ShouldBe(Key.Error);
                errorMessage = _stringConverter.Read(reader);
            }
            else if (length.Value > 0u)
            {
                _keyConverter.Read(reader).ShouldBe(Key.Data);
                data = _dataConverter.Read(reader);
            }

            return new ResponsePacket<T>(header, errorMessage, data);
        }
    }
}