using iproto.Data.Packets;

namespace iproto.Interfaces
{
    public interface IResponseReader<T>
    {
        UnifiedPacket ReadResponse(byte[] response);
    }
}