namespace iproto.Data.Packets
{
    public class GreetingsPacket
    {
        public GreetingsPacket(byte[] message, byte[] salt)
        {
            Message = message;
            Salt = salt;
        }

        public byte[] Message { get; }

        public byte[] Salt { get; }
    }
}