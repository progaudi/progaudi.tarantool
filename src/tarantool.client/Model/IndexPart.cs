using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model
{
    public class IndexPart
    {
        public IndexPart(uint fieldNo, IndexPartType type)
        {
            FieldNo = fieldNo;
            Type = type;
        }

        public uint FieldNo { get; }

        public IndexPartType Type { get; }
    }
}