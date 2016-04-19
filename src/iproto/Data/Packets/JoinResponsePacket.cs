using System.Collections.Generic;

namespace iproto.Data.Packets
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