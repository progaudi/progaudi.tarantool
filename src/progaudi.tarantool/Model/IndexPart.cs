using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model
{
    public class IndexPart
    {
        public IndexPart(uint fieldNo, string type)
        {
            FieldNo = fieldNo;
            Type = type;
        }

        public uint FieldNo { get; }

        public string Type { get; }
    }
}