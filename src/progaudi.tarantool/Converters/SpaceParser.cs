using System;
using System.Collections.Generic;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class SpaceParser : IMsgPackParser<Space>
    {
        private readonly IMsgPackParser<StorageEngine> _engineConverter;
        private readonly IMsgPackParser<List<SpaceField>> _fieldConverter;

        public SpaceParser(MsgPackContext context)
        {
            _engineConverter = context.GetRequiredParser<StorageEngine>();
            _fieldConverter = context.GetRequiredParser<List<SpaceField>>();
        }

        public Space Parse(ReadOnlySpan<byte> source, out int readSize)
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
            readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).Length;

            var name = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp;
            var engine = _engineConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
            var fieldCount = MsgPackSpec.ReadUInt32(source.Slice(readSize), out temp); readSize += temp;
            
            //TODO Find what skipped dictionary used for
            readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).Length;

            var fields = _fieldConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
            
            return new Space(id, fieldCount, name, engine, fields.AsReadOnly());
        }
    }
}