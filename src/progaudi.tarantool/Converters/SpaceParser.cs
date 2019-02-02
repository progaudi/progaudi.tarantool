using System.Buffers;
using System.Collections.Generic;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class SpaceParser : IMsgPackSequenceParser<Space>
    {
        private readonly MsgPackContext _context;
        private readonly IMsgPackSequenceParser<List<SpaceField>> _fieldConverter;

        public SpaceParser(MsgPackContext context)
        {
            _context = context;
            _fieldConverter = context.GetRequiredSequenceParser<List<SpaceField>>();
        }

        public Space Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            var actual = MsgPackSpec.ReadArrayHeader(source, out readSize);
            const uint expected = 7u;
            if (actual != expected)
            {
                throw ExceptionHelper.InvalidArrayLength(expected, actual);
            }

            // ReSharper disable once InlineOutVariableDeclaration
            int temp;
            var id = MsgPackSpec.ReadUInt32(source.Slice(readSize), out temp); readSize += temp;

            //TODO Find what skipped number means
            readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).GetIntLength();

            var name = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp;
            var engine = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp;
            var fieldCount = MsgPackSpec.ReadUInt32(source.Slice(readSize), out temp); readSize += temp;
            
            //TODO Find what skipped dictionary used for
            readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).GetIntLength();

            var fields = _fieldConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
            
            return new Space(id, fieldCount, name, engine, fields.AsReadOnly(), _context);
        }
    }
}