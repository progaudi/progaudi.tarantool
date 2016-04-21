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
            result.RegisterConverter(new UpdateOperationConverter<sbyte>());
            result.RegisterConverter(new UpdateOperationConverter<byte>());
            result.RegisterConverter(new UpdateOperationConverter<ushort>());
            result.RegisterConverter(new UpdateOperationConverter<short>());
            result.RegisterConverter(new UpdateOperationConverter<uint>());
            result.RegisterConverter(new UpdateOperationConverter<int>());
            result.RegisterConverter(new UpdateOperationConverter<ulong>());
            result.RegisterConverter(new UpdateOperationConverter<long>());
            result.RegisterConverter(new UpdateOperationConverter<object>());

            result.RegisterConverter(new ReflectionConverter());
            result.RegisterConverter(new Tuple1Converter<object[]>());
            result.RegisterConverter(new HeaderConverter());

            result.RegisterConverter(new AuthenticationPacketConverter());
            result.RegisterConverter(new ResponsePacketConverter());

            return result;
        } 
    }
}