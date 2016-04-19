using TarantoolDnx.MsgPack;

namespace iproto.Data.Request
{
    public interface IRequestBody
    {
        void WriteTo(IMsgPackWriter mspgMsgPackWriter);
    }
}