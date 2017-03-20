using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public class ClientOptions
    {
        public ClientOptions(ILog log = null, MsgPackContext context = null)
            : this(new ConnectionOptions(), log, context)
        {
        }

        public ClientOptions(string replicationSource, ILog log = null, MsgPackContext context = null)
            : this(new ConnectionOptions(replicationSource, log), log, context)
        {
        }

        private ClientOptions(ConnectionOptions options, ILog log, MsgPackContext context)
        {
            ConnectionOptions = options;
            LogWriter = log;
            MsgPackContext = context ?? new MsgPackContext();
        }

        public ILog LogWriter { get; }

        public MsgPackContext MsgPackContext { get; }

        public ConnectionOptions ConnectionOptions { get; }
    }
}