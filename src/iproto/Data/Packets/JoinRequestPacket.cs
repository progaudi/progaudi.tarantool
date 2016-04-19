namespace iproto.Data.Packets
{
    public class JoinPacket
    {
        public JoinPacket(int sync, string serverUuid)
        {
            Sync = sync;
            ServerUuid = serverUuid;
        }

        public int Sync { get; }

        public string ServerUuid { get; }
    }
}