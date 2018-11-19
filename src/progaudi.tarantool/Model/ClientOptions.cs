using ProGaudi.MsgPack;

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
            MsgPackContext = context ?? new MsgPackContext(/*binaryCompatibilityMode: true*/);
            if (log != null)
            {
                LogWriter = new LogWriterWrapper(this, log);
            }
        }

        public ILog LogWriter { get; }

        public MsgPackContext MsgPackContext { get; }

        public ConnectionOptions ConnectionOptions { get; }

        public string Name { get; set; }

        private class LogWriterWrapper : ILog
        {
            private readonly ClientOptions _options;
            private readonly ILog _log;

            public LogWriterWrapper(ClientOptions options, ILog log)
            {
                _options = options;
                _log = log;
            }

            public void WriteLine(string message)
            {
                _log.WriteLine($"[{_options.Name}] {message}");
            }

            public void Flush()
            {
                _log.Flush();
            }
        }
    }
}