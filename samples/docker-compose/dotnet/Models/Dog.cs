using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;

namespace dotnet.Models
{
    [MsgPackArray]
    public class Dog
    {
        private MsgPackToken _addditionalDataRaw;

        [MsgPackArrayElement(0)]
        public long Id { get; set; }

        [MsgPackArrayElement(1)]
        public string Name { get; set; }

        [MsgPackArrayElement(2)]
        public long Age { get; set; }

        [MsgPackArrayElement(3)]
        public MsgPackToken AddditionalDataRaw
        {
            get => _addditionalDataRaw;
            set
            {
                _addditionalDataRaw = value;
                var _ = TryParseString(value) || TryParseInt(value);
            }
        }

        public string AdditionalData { get; private set; }

        private bool TryParseString(MsgPackToken token)
        {
            try
            {
                AdditionalData = (string) token;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryParseInt(MsgPackToken token)
        {
            try
            {
                AdditionalData = ((double) token).ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}