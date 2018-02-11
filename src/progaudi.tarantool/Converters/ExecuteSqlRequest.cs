using System;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ExecuteSqlRequestConverter : IMsgPackConverter<ExecuteSqlRequest>
    {
        private MsgPackContext _context;
        private bool _initialized;
        private IMsgPackConverter<object> _nullConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<Key> _keyConverter;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(ExecuteSqlRequest value, IMsgPackWriter writer)
        {
            if (!_initialized)
            {
                InitializeIfNeeded();
            }

            writer.WriteMapHeader(3u);

            _keyConverter.Write(Key.SqlQueryText, writer);
            _stringConverter.Write(value.Query, writer);

            _keyConverter.Write(Key.SqlParameters, writer);
            writer.WriteArrayHeader((uint) value.Parameters.Count);
            foreach (var parameter in value.Parameters)
            {
                parameter.Write(_context, writer, _stringConverter);
            }

            _keyConverter.Write(Key.SqlOptions, writer);
            _nullConverter.Write(null, writer);
        }

        public ExecuteSqlRequest Read(IMsgPackReader reader)
        {
            throw new NotSupportedException();
        }

        private void InitializeIfNeeded()
        {
            _initialized = true;
            _nullConverter = _context.NullConverter;
            _stringConverter = _context.GetConverter<string>();
            _keyConverter = _context.GetConverter<Key>();
        }
    }
}