namespace iproto.Data.Packets
{
    public class SubscribePacket
    {
        public SubscribePacket(int sync, string serverUuid, string clusterUid, int vclock)
        {
            Sync = sync;
            ServerUuid = serverUuid;
            ClusterUid = clusterUid;
            Vclock = vclock;
        }

        public int Sync { get; }

        public string ServerUuid { get; }

        public string ClusterUid { get; }

        public int Vclock { get; }
    }
}