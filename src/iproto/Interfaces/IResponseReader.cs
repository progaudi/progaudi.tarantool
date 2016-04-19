using iproto.Data.Packets;
using TarantoolDnx.MsgPack;

namespace iproto.Interfaces
{
    public interface IResponseReader
    {
        UnifiedPacket ReadResponse(IMsgPackReader reader);

        GreetingsPacket ReadGreetings(byte[] response);
    }
}