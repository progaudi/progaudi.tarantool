using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;

namespace dotnet.Models
{
    public class Dog
    {
        public Dog(TarantoolTuple<long, string, long, MsgPackToken> tuple)
        {
            Id = tuple.Item1;
            Name = tuple.Item2;
            Age = tuple.Item3;
            var _ = TryParseString(tuple.Item4) || TryParseInt(tuple.Item4);
        }

        public long Id { get; }

        public string Name { get; }

        public long Age { get; }

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