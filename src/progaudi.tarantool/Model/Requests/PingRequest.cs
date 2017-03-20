using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class PingRequest : IRequest
    {
        public CommandCode Code => CommandCode.Ping;
    }
}