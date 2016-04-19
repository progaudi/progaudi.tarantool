using TarantoolDnx.MsgPack;

namespace iproto.Data.Bodies
{
    public class ErrorBody : IBody
    {
        public ErrorBody(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }


        public void WriteTo(IMsgPackWriter mspgMsgPackWriter)
        {
            throw new System.NotImplementedException();
        }
    }
}