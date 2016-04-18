using System.Collections.Generic;
using TarantoolDnx.MsgPack;
using TarantoolDnx.MsgPack.Converters;

namespace iproto.Data
{
    public class Header
    {
        public Header(CommandCode code, ulong sync, ulong schemaId)
        {
            Code = code;
            Sync = sync;
            SchemaId = schemaId;
        }

        public CommandCode Code { get; }

        public ulong Sync { get; }

        public ulong SchemaId { get; }

        public bool IsError => (Code & CommandCode.ErrorMask) == CommandCode.ErrorMask;

        public byte[] Serialize(MsgPackContext msgPackContext)
        {
            var headerMap = new Dictionary<Key, ulong>
            {
                {Key.Code, (ulong) Code},
                {Key.Sync, Sync},
                {Key.SchemaId, SchemaId}
            };

            return MsgPackConverter.Serialize(headerMap, msgPackContext);
        }
    }
}