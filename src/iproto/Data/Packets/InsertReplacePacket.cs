using System;

namespace iproto.Data.Packets
{
    public class InsertReplacePacket<T1> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3, T4> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5, T6> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public InsertReplacePacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}