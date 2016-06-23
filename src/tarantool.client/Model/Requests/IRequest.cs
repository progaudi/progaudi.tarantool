using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model.Requests
{
    public interface IRequest
    {
        CommandCode Code { get; }
    }
}