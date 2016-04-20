using System;
using System.Collections.Generic;

using Shouldly;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class ResponsePacketConverter : IMsgPackConverter<ResponsePacket>
    {
        public void Write(ResponsePacket value, IBytesWriter writer, MsgPackContext context)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket Read(IBytesReader reader, MsgPackContext context, Func<ResponsePacket> creator)
        {
            var headerConverter = context.GetConverter<Header>();
            var keyConverter = context.GetConverter<Key>();

            var header = headerConverter.Read(reader, context, null);

            uint length;
            reader.ReadMapLengthOrNull(out length).ShouldBe(true);
            length.ShouldBe(1u);

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var stringConverter = context.GetConverter<string>();

                keyConverter.Read(reader, context, null).ShouldBe(Key.Error);
                var errorMessage = stringConverter.Read(reader, context, null);

                return new ResponsePacket(header, errorMessage, null);
            }

            var dataConverter = context.GetConverter<object>();

            keyConverter.Read(reader, context, null).ShouldBe(Key.Error);
            var data = dataConverter.Read(reader, context, null);

            return new ResponsePacket(header, null, data);
        }
    }
}