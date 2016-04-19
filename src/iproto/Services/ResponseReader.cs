using System;
using System.Collections.Generic;
using iproto.Data.Packets;
using iproto.Interfaces;
using System.Text;
using iproto.Data;
using iproto.Data.Bodies;
using TarantoolDnx.MsgPack;
using TarantoolDnx.MsgPack.Converters;

namespace iproto.Services
{
    public class ResponseReader : IResponseReader
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        public UnifiedPacket ReadResponse(IMsgPackReader reader)
        {
            var bodyAndHeaderSize = reader.Read<ulong>();

            var headerMap = reader.Read<Dictionary<Key, ulong>>();
            var bodyMap = reader.Read<Dictionary<Key, string>>();

            var header = new Header(
                (CommandCode) headerMap[Key.Code],
                headerMap[Key.Sync],
                headerMap[Key.SchemaId]);

            IBody body = null;
            if (header.IsError)
            {
                body = new ErrorBody(bodyMap[Key.Error]);
            }
            return new UnifiedPacket(header, body);
        }

        public GreetingsPacket ReadGreetings(byte[] response)
        {
            var message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            var decodedSalt= Convert.FromBase64String(saltString);

            return new GreetingsPacket(message, decodedSalt);
        }
    }
}