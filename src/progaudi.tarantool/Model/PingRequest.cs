namespace ProGaudi.Tarantool.Client.Model
{
    public class PingRequest : IRequest
    {
        public CommandCodes Code => CommandCodes.Ping;
    }
}