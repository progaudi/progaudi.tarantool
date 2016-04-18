using iproto.Data.Packets;
using TarantoolDnx.MsgPack;

namespace iproto.Interfaces
{
    public interface IResponseReader
    {
        UnifiedPacket ReadResponse(byte[] response, MsgPackContext msgPackContext);

        GreetingsPacket ReadGreetings(byte[] response);
    }
}