using System;
using System.Collections.Generic;

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

            var header = headerConverter.Read(reader, context, null);

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorConverter = context.GetConverter<Dictionary<Key, string>>();
                var errorBody = errorConverter.Read(reader, context, null);
                return new ResponsePacket(header, errorBody[Key.Error], null);
            }

            var dataConverter = context.GetConverter<Dictionary<Key, object>>();
            var dataBody = dataConverter.Read(reader, context, null);
            return new ResponsePacket(header, null, dataBody[Key.Data]);
        }
    }
}