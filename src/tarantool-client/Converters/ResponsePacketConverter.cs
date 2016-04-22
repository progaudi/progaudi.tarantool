using System;

using Shouldly;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class ResponsePacketConverter : IMsgPackConverter<ResponsePacket>
    {
        public void Write(ResponsePacket value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket Read(IMsgPackReader reader, MsgPackContext context, Func<ResponsePacket> creator)
        {
            var headerConverter = context.GetConverter<Header>();
            var keyConverter = context.GetConverter<Key>();

            var header = headerConverter.Read(reader, context, null);
            string errorMessage = null;
            object data = null;

            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                length.ShouldBe(1u);

                var stringConverter = context.GetConverter<string>();

                keyConverter.Read(reader, context, null).ShouldBe(Key.Error);
                errorMessage = stringConverter.Read(reader, context, null);
            }

            if (length.Value > 0u)
            {
                var dataConverter = context.GetConverter<object>();

                keyConverter.Read(reader, context, null).ShouldBe(Key.Error);
                data = dataConverter.Read(reader, context, null);
            }

            return new ResponsePacket(header, errorMessage, data);
        }
    }
}