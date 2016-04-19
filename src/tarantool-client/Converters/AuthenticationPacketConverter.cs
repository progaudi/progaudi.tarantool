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
            }
            else
            {
                var headerConverter = context.GetConverter<Header>();
                headerConverter.Write(value.Header, writer, context);

                var bodyMapConverter = context.GetConverter<Dictionary<Key, object>>();
                var bodyMap = new Dictionary<Key, object>()
                {
                    {Key.Username, value.Username},
                    {
                        Key.Tuple, Tuple.Create(
                            new object[]
                            {
                                "chap-sha1",
                                value.Scramble
                            })
                    }
                };
                bodyMapConverter.Write(bodyMap, writer, context);
            }
        }

        public AuthenticationPacket Read(IBytesReader reader, MsgPackContext context, Func<AuthenticationPacket> creator)
        {
            throw new NotImplementedException();
        }
    }
}