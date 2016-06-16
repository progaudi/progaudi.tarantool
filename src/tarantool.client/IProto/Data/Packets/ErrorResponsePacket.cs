namespace Tarantool.Client.IProto.Data.Packets
{
    public class ErrorResponsePacket
    {
        public ErrorResponsePacket(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}