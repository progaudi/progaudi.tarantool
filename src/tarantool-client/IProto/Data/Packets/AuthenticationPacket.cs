namespace tarantool_client.IProto.Data.Packets
{
    public class AuthenticationPacket : UnifiedPacket
    {
        public AuthenticationPacket(string username, byte[] scramble)
            : base(new Header(CommandCode.Auth, null, null))
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }
    }
}