using System.IO;

namespace ProGaudi.Tarantool.Client
{
    public class StringWriterLog : ILog
    {
        private readonly TextWriter _internalWriter;

        public StringWriterLog()
        {
            StringWriter = new StringWriter();

            _internalWriter = StringWriter;
        }

        public StringWriter StringWriter { get; }

        public void WriteLine(string message)
        {
            _internalWriter.WriteLine(message);
        }

        public void Flush()
        {
            _internalWriter.Flush();
        }
    }
}