using iproto.Data;

using tarantool_client.Converters;

using TarantoolDnx.MsgPack;

namespace tarantool_client
{
    public class MsgPackContextFactory
    {
        public static MsgPackContext Create()
        {
            var result = new MsgPackContext();

            result.RegisterConverter(new EnumConverter<Key>());
            result.RegisterConverter(new EnumConverter<CommandCode>());
            result.RegisterConverter(new EnumConverter<Iterator>());

            result.RegisterConverter(new StringSliceOperationConverter());

            result.RegisterGenericConverter(typeof(UpdateOperationConverter<>));

            result.RegisterConverter(new ReflectionConverter());
            result.RegisterConverter(new HeaderConverter());

            result.RegisterConverter(new AuthenticationPacketConverter());
            result.RegisterConverter(new ResponsePacketConverter());

            result.RegisterGenericConverter(typeof(TupleConverter<>));
            result.RegisterGenericConverter(typeof(TupleConverter<,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));

            return result;
        }
    }
}