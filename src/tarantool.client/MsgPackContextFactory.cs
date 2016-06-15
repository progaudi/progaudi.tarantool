using MsgPack.Light;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Converters;

namespace Tarantool.Client
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