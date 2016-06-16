namespace Tarantool.Client.IProto.Data.Packets
{
    public class ResponsePacket<T>
    { 
        public ResponsePacket(T data) 
        {
            Data = data;
        }

        public T Data { get; }
    }
}