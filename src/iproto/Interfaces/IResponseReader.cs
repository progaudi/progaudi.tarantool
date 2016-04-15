using iproto.Data.Packets;

namespace iproto.Interfaces
{
    public interface IResponseReader
    {
        UnifiedPacket ReadResponse(byte[] response);

        GreetingsPacket ReadGreetings(byte[] response);
    }
}