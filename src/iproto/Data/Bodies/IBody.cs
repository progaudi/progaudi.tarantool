using TarantoolDnx.MsgPack;

namespace iproto.Data.Bodies
{
    public interface IBody
    {
        byte[] Serialize(MsgPackContext msgPackContext);
    }
}