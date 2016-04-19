using System;
using System.Collections.Generic;
using iproto.Data.Packets;
using iproto.Interfaces;
using System.Text;
using iproto.Data;

using TarantoolDnx.MsgPack;

namespace iproto.Services
{
    public class ResponseReader : IResponseReader
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        public ResponsePacket ReadResponse(IMsgPackReader reader)
        {
            var bodyAndHeaderSize = reader.Read<ulong>();

            var headerMap = reader.Read<Dictionary<Key, ulong>>();
            var bodyMap = reader.Read<Dictionary<Key, string>>();

            var header = new Header(
                (CommandCode) headerMap[Key.Code],
                headerMap[Key.Sync],
                headerMap[Key.SchemaId]);

            return IsErrorResponse(header) ? new ResponsePacket(header, bodyMap[Key.Error], null) : new ResponsePacket(header, null, bodyMap[Key.Data]);
        }

        
        public GreetingsPacket ReadGreetings(byte[] response)
        {
            var message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            var decodedSalt= Convert.FromBase64String(saltString);

            return new GreetingsPacket(message, decodedSalt);
        }

        private bool IsErrorResponse(Header header)
        {
            return (header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask;
        }
    }
}