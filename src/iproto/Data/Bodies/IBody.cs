using TarantoolDnx.MsgPack;

namespace iproto.Data.Bodies
{
    public interface IBody
    {
        void WriteTo(IMsgPackWriter mspgMsgPackWriter);
    }
}