using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public interface IRequest
    {
        CommandCode Code { get; }
    }
}