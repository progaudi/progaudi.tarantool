using MsgPack.Light;

using tarantool_client.IProto;
using tarantool_client.IProto.Converters;

namespace tarantool_client
{
    public class MsgPackContextFactory
    {
        public static MsgPackContext Create()
        {
            var msgPackContext = new MsgPackContext();

            ConverterRegistrator.Register(msgPackContext);

            msgPackContext.RegisterConverter(new FromStringEnumConverter<StorageEngine>());
            msgPackContext.RegisterConverter(new FromStringEnumConverter<FieldType>());
            msgPackContext.RegisterConverter(new FromStringEnumConverter<IndexPartType>());
            msgPackContext.RegisterConverter(new FromStringEnumConverter<IndexType>());

            return msgPackContext;
        }
    }
}