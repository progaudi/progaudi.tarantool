using iproto.Data;

using tarantool_client.Converters;

using MsgPack.Light;

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

            result.RegisterConverter(new FromStringEnumConverter<StorageEngine>());
            result.RegisterConverter(new FromStringEnumConverter<FieldType>());
            result.RegisterConverter(new FromStringEnumConverter<IndexPartType>());
            result.RegisterConverter(new FromStringEnumConverter<IndexType>());

            result.RegisterConverter(new StringSliceOperationConverter());
            result.RegisterGenericConverter(typeof(UpdateOperationConverter<>));

            result.RegisterConverter(new HeaderConverter());

            result.RegisterConverter(new AuthenticationPacketConverter());
            result.RegisterConverter(new JointRequestConverter());
            result.RegisterConverter(new JoinResponsePacketConverter());
            result.RegisterConverter(new SubscribePacketConverter());

            result.RegisterConverter(new SpaceFieldConverter());
            result.RegisterConverter(new SpaceConverter());
            result.RegisterConverter(new IndexPartConverter());
            result.RegisterConverter(new IndexCreationOptionsConverter());
            result.RegisterConverter(new IndexConverter());
            result.RegisterConverter(new TupleConverter());

            result.RegisterGenericConverter(typeof(ResponsePacketConverter<>));
            result.RegisterGenericConverter(typeof(UpdatePacketConverter<,>));
            result.RegisterGenericConverter(typeof(CallPacketConverter<>));
            result.RegisterGenericConverter(typeof(DeletePacketConverter<>));
            result.RegisterGenericConverter(typeof(EvalPacketConverter<>));
            result.RegisterGenericConverter(typeof(InsertReplacePacketConverter<>));
            result.RegisterGenericConverter(typeof(SelectPacketConverter<>));
            result.RegisterGenericConverter(typeof(UpsertPacketConverter<,>));

            result.RegisterGenericConverter(typeof(TupleConverter<>));
            result.RegisterGenericConverter(typeof(TupleConverter<,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));
            result.RegisterGenericConverter(typeof(TupleConverter<,,,,,,,>));

            return result;
        }
    }
}