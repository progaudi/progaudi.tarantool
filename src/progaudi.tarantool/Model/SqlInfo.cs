using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
    public class SqlInfo
    {
        public SqlInfo(int rowCount)
        {
            RowCount = rowCount;
        }

        public int RowCount { get; }

        public sealed class Formatter : IMessagePackFormatter<SqlInfo>
        {
            public int Serialize(ref byte[] bytes, int offset, SqlInfo value, IFormatterResolver formatterResolver)
            {
                throw new System.NotImplementedException();
            }

            public SqlInfo Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                if (MessagePackBinary.IsNil(bytes, offset))
                {
                    readSize = 1;
                    return null;
                }

                var startOffset = offset;
                var length = MessagePackBinary.ReadMapHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                var result = default(SqlInfo);
                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
                    switch (key)
                    {
                        case Keys.SqlRowCount:
                            result = new SqlInfo(MessagePackBinary.ReadInt32(bytes, offset, out readSize));
                            offset += readSize;
                            break;
                        default:
                            offset += MessagePackBinary.ReadNext(bytes, offset);
                            break;
                    }
                }

                readSize = offset - startOffset;

                return result;
            }
        }
    }
}