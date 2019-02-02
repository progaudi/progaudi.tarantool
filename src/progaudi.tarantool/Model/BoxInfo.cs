using System;
using System.Buffers;
using ProGaudi.MsgPack;

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

        public class Converter : IMsgPackSequenceParser<BoxInfo>
        {
            public BoxInfo Parse(ReadOnlySequence<byte> source, out int readSize)
            {
                if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

                var mapLength = MsgPackSpec.ReadMapHeader(source, out readSize);

                var result = new BoxInfo();
                for (var i = 0; i < mapLength; i++)
                {
                    var propertyName = MsgPackSpec.ReadString(source, out var temp);
                    readSize += temp;
                    switch (propertyName)
                    {
                        case "id":
                            result.Id = MsgPackSpec.ReadInt32(source, out temp);
                            readSize += temp;
                            break;
                        case "lsn":
                            result.Lsn = MsgPackSpec.ReadInt32(source, out temp);
                            readSize += temp;
                            break;
                        case "pid":
                            result.Pid = MsgPackSpec.ReadInt32(source, out temp);
                            readSize += temp;
                            break;
                        case "ro":
                            result.ReadOnly = MsgPackSpec.ReadBoolean(source, out temp);
                            readSize += temp;
                            break;
                        case "uuid":
                            result.Uuid = Guid.Parse(MsgPackSpec.ReadString(source, out temp));
                            readSize += temp;
                            break;
                        case "version":
                            result.Version = TarantoolVersion.Parse(MsgPackSpec.ReadString(source, out temp));
                            readSize += temp;
                            break;
                        default:
                            readSize += MsgPackSpec.ReadToken(source).GetIntLength();
                            break;
                    }
                }

                return result;
            }
        }
    }
}