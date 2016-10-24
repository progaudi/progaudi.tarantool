using System.IO;

namespace ProGaudi.Tarantool.Client
{
    public class StringWriterLog : TextWriterLog
    {
        public StringWriterLog()
            : base(new StringWriter())
        {
        }

        public StringWriter StringWriter => (StringWriter)InternalWriter;
    }
}