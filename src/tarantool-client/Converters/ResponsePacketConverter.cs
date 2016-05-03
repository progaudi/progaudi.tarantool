using System;

using Shouldly;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class ResponsePacketConverter<T> : IMsgPackConverter<ResponsePacket<T>>
    {
        public void Write(ResponsePacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<ResponsePacket<T>> creator)
        {
            var headerConverter = context.GetConverter<Header>();
            var keyConverter = context.GetConverter<Key>();

            var header = headerConverter.Read(reader, context, null);
            string errorMessage = null;
            T data = default(T);

            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                length.ShouldBe(1u);

                var stringConverter = context.GetConverter<string>();

                keyConverter.Read(reader, context, null).ShouldBe(Key.Error);
                errorMessage = stringConverter.Read(reader, context, null);
            }
            else if (length.Value > 0u)
            {
                var dataConverter = context.GetConverter<T>();

                keyConverter.Read(reader, context, null).ShouldBe(Key.Data);
                data = dataConverter.Read(reader, context, null);
            }

            return new ResponsePacket<T>(header, errorMessage, data);
        }
    }
}