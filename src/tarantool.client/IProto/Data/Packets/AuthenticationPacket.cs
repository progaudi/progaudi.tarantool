namespace Tarantool.Client.IProto.Data.Packets
{
    public class AuthenticationPacket : IRequestPacket
    {
        public AuthenticationPacket(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        public string Username { get; }

        public byte[] Scramble { get; }

        public CommandCode Code => CommandCode.Auth;
    }
}