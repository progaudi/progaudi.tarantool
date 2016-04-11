using iproto.Data.Packets;

namespace iproto.Interfaces
{
    public interface IGreetingsReader
    {
        GreetingsPacket Read(byte[] response);
    }
}