using System;
using System.Collections.Generic;
using TarantoolDnx.MsgPack;
using TarantoolDnx.MsgPack.Converters;

namespace iproto.Data.Bodies
{
    public class AuthenticationBody : IBody
    {
        public AuthenticationBody(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }

        public byte[] Serialize(MsgPackContext msgPackContext)
        {
            var bodyMap = new Dictionary<Key, object>()
            {
                {Key.Username, Username},
                {
                    Key.Tuple, Tuple.Create(
                        new object[]
                        {
                            "chap-sha1",
                            Scramble
                        })
                }
            };

            return MsgPackConverter.Serialize(bodyMap, msgPackContext);
        }
    }
}