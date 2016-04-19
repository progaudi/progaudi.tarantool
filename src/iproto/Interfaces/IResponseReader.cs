using iproto.Data.Packets;
using TarantoolDnx.MsgPack;

namespace iproto.Interfaces
{
    public interface IResponseReader
    {
        ResponsePacket ReadResponse(IMsgPackReader reader);

        GreetingsPacket ReadGreetings(byte[] response);
    }
}