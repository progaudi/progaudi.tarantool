using System;

namespace iproto.Data.Packets
{
    public class DeletePacket<T1> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1> Key { get; }
    }
    public class DeletePacket<T1, T2> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2> Key { get; }
    }

    public class DeletePacket<T1, T2, T3> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3> Key { get; }
    }

    public class DeletePacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3, T4> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3, T4> Key { get; }
    }
    public class DeletePacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3, T4, T5> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5> Key { get; }
    }

    public class DeletePacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3, T4, T5, T6> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Key { get; }
    }

    public class DeletePacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3, T4, T5, T6, T7> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Key { get; }
    }

    public class DeletePacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public DeletePacket(uint spaceId, uint indexId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> key)
           : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Key { get; }
    }
}