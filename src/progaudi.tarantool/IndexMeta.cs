using System.Collections.Generic;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client
{
    // TODO: fix serializer to support struct.
    [MsgPackArray]
    public sealed class IndexMeta
    {
        [MsgPackArrayElement(1)]
        public uint Id { get; set; }

        [MsgPackArrayElement(0)]
        public uint SpaceId { get; set; }

        [MsgPackArrayElement(2)]
        public string Name { get; set; }

        [MsgPackArrayElement(4)]
        public IndexCreationOptions Options { get; set; }

        [MsgPackArrayElement(3)]
        public IndexType Type { get; set; }

        [MsgPackArrayElement(5)]
        public IReadOnlyList<IndexPart> Parts { get; set; }
    }
}