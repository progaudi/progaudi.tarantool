namespace iproto.Data.Packets
{
    public class AuthenticationPacket : UnifiedPacket
    {
        public AuthenticationPacket(Header header, string username, byte[] scramble)
            : base(header)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }
    }
}