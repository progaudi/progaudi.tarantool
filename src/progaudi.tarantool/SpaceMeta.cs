using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client
{
    // TODO: fix serializer to support struct.
    [MsgPackArray]
    public class SpaceMeta
    {
        [MsgPackArrayElement(0)]
        public uint Id { get; set; }

        [MsgPackArrayElement(1)]
        public int OwnerId { get; set; }

        [MsgPackArrayElement(2)]
        public string Name { get; set; }

        [MsgPackArrayElement(3)]
        public StorageEngine Engine { get; set; }

        [MsgPackArrayElement(4)]
        public uint FieldCount { get; set; }

        [MsgPackArrayElement(5)]
        public SpaceOptions Options { get; set; }

        [MsgPackArrayElement(6)]
        public SpaceField[] Fields { get; set; }
    }
}