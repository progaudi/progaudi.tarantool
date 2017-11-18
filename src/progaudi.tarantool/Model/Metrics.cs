namespace ProGaudi.Tarantool.Client.Model
{
    public class Metrics
    {
        private readonly ILogicalConnection _logicalConnection;

        public Metrics(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public uint PingsFailedByTimeoutCount => _logicalConnection.PingsFailedByTimeoutCount;
    }
}
