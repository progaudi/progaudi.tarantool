using System.Collections.Generic;

namespace tarantool_client.IProto.Data.Packets
{
    public class JoinResponsePacket
    {
        public JoinResponsePacket(int sync, Dictionary<int, int> srvId2SrvLsn)
        {
            Sync = sync;
            SrvId2SrvLsn = srvId2SrvLsn;
        }

        public int Sync { get; }

        public Dictionary<int, int> SrvId2SrvLsn { get; }
    }
}