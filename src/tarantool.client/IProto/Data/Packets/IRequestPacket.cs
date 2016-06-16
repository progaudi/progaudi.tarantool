namespace Tarantool.Client.IProto.Data.Packets
{
    public interface IRequestPacket
    {
        CommandCode Code { get; }
    }
}