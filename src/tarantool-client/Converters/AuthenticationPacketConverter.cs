using System;
using System.Collections.Generic;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class AuthenticationPacketConverter : IMsgPackConverter<AuthenticationPacket>
    {
        public void Write(AuthenticationPacket value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(null, writer, context);
                return;
            }

            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var stringConverter = context.GetConverter<string>();
            var bytesConverter = context.GetConverter<byte[]>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.Username, writer, context);
            stringConverter.Write(value.Username, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);

            writer.WriteArrayHeaderAndLength(2);
            stringConverter.Write("chap-sha1", writer, context);
            bytesConverter.Write(value.Scramble, writer, context);
        }

        public AuthenticationPacket Read(IBytesReader reader, MsgPackContext context, Func<AuthenticationPacket> creator)
        {
            throw new NotImplementedException();
        }
    }
}