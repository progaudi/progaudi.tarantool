using System;
using System.Collections.Generic;

using iproto.Data.Request;

using TarantoolDnx.MsgPack;

namespace iproto.Data.Bodies
{
    public class AuthenticationBody : IRequestBody
    {
        public AuthenticationBody(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }

        public void WriteTo(IMsgPackWriter msgPackWriter)
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

            msgPackWriter.Write(bodyMap);
        }
    }
}