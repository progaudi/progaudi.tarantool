using tarantool_client.Converters;

using TarantoolDnx.MsgPack;

namespace tarantool_client
{
    public class MsgPackContextFactory
    {
        public static MsgPackContext Create()
        {
            var resutl = new MsgPackContext();
            resutl.RegisterConverter(new KeyConverter());
            resutl.RegisterConverter(new ReflectionConverter());
            resutl.RegisterConverter(new Tuple1Converter<object[]>());

            return resutl;
        } 
    }
}