namespace ProGaudi.Tarantool.Client.Model
{
    public class ClientOptions
    {
        public ClientOptions(ILog log = null)
            : this(new ConnectionOptions(), log)
        {
        }

        public ClientOptions(string replicationSource, ILog log = null)
            : this(new ConnectionOptions(replicationSource, log), log)
        {
        }

        private ClientOptions(ConnectionOptions options, ILog log)
        {
            ConnectionOptions = options;
            if (log != null)
            {
                LogWriter = new LogWriterWrapper(this, log);
            }
        }

        public ILog LogWriter { get; }

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