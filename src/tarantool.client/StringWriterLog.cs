using System.IO;

namespace Tarantool.Client
{
    public class StringWriterLog : ILog
    {
        private readonly TextWriter _internalWriter = TextWriter.Synchronized(new StringWriter());

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