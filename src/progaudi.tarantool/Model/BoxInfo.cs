using System;
using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public class BoxInfo
    {
        public long Id { get; private set; }

        public long Lsn { get; private set; }

        public long Pid { get; private set; }

        public bool ReadOnly { get; private set; }

        public Guid Uuid { get; private set; }

        public TarantoolVersion Version { get; private set; }

        public class Converter : IMsgPackConverter<BoxInfo>
        {
            private IMsgPackConverter<string> _stringConverter;
            private MsgPackContext _context;
            private bool _initialized;
            private IMsgPackConverter<long> _longConverter;
            private IMsgPackConverter<bool> _boolConverter;

            public void Initialize(MsgPackContext context)
            {
                _context = context;
            }

            public void Write(BoxInfo value, IMsgPackWriter writer)
            {
                throw new NotSupportedException();
            }

            public BoxInfo Read(IMsgPackReader reader)
            {
                if (!_initialized)
                {
                    InitializeIfNeeded();
                }

                var mapLength = reader.ReadMapLength();
                if (!mapLength.HasValue)
                {
                    return null;
                }

                var result = new BoxInfo();
                for (var i = 0; i < mapLength; i++)
                {
                    switch (_stringConverter.Read(reader))
                    {
                        case "id":
                            result.Id = _longConverter.Read(reader);
                            break;
                        case "lsn":
                            result.Lsn = _longConverter.Read(reader);
                            break;
                        case "pid":
                            result.Pid = _longConverter.Read(reader);
                            break;
                        case "ro":
                            result.ReadOnly = _boolConverter.Read(reader);
                            break;
                        case "uuid":
                            result.Uuid = Guid.Parse(_stringConverter.Read(reader));
                            break;
                        case "version":
                            result.Version = TarantoolVersion.Parse(_stringConverter.Read(reader));
                            break;
                        default:
                            reader.SkipToken();
                            break;
                    }
                }

                return result;
            }

            private void InitializeIfNeeded()
            {
                _initialized = true;
                _stringConverter = _context.GetConverter<string>();
                _longConverter = _context.GetConverter<long>();
                _boolConverter = _context.GetConverter<bool>();
            }
        }
    }
}