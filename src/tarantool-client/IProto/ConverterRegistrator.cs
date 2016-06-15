using MsgPack.Light;

using tarantool_client.IProto.Converters;
using tarantool_client.IProto.Data;

namespace tarantool_client.IProto
{
    public class ConverterRegistrator
    {
        public static MsgPackContext Register(MsgPackContext msgPackContext)
        {
            msgPackContext.RegisterConverter(new EnumConverter<Key>());
            msgPackContext.RegisterConverter(new EnumConverter<CommandCode>());
            msgPackContext.RegisterConverter(new EnumConverter<Iterator>());
            msgPackContext.RegisterConverter(new StringSliceOperationConverter());
            msgPackContext.RegisterGenericConverter(typeof(UpdateOperationConverter<>));
            msgPackContext.RegisterConverter(new HeaderConverter());
            msgPackContext.RegisterConverter(new AuthenticationPacketConverter());
            msgPackContext.RegisterConverter(new JointRequestConverter());
            msgPackContext.RegisterConverter(new JoinResponsePacketConverter());
            msgPackContext.RegisterConverter(new SubscribePacketConverter());
            msgPackContext.RegisterConverter(new SpaceFieldConverter());
            msgPackContext.RegisterConverter(new SpaceConverter());
            msgPackContext.RegisterConverter(new IndexPartConverter());
            msgPackContext.RegisterConverter(new IndexCreationOptionsConverter());
            msgPackContext.RegisterConverter(new IndexConverter());
            msgPackContext.RegisterConverter(new TupleConverter());
            msgPackContext.RegisterGenericConverter(typeof(ResponsePacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(UpdatePacketConverter<,>));
            msgPackContext.RegisterGenericConverter(typeof(CallPacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(DeletePacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(EvalPacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(InsertReplacePacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(SelectPacketConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(UpsertPacketConverter<,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,,,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,,,,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));
            msgPackContext.RegisterGenericConverter(typeof(TupleConverter<,,,,,,,>));

            return msgPackContext;
        }
    }
}