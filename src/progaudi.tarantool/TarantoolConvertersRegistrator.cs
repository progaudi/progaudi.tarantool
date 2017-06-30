using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Converters;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client
{
    public class TarantoolConvertersRegistrator
    {
        public static void Register(MsgPackContext context)
        {
            context.RegisterConverter(new EnumConverter<Key>());
            context.RegisterConverter(new EnumConverter<CommandCode>());
            context.RegisterConverter(new EnumConverter<Iterator>());
            context.RegisterConverter(new RequestIdConverter());
            context.RegisterConverter(new PacketSizeConverter());

            context.RegisterConverter(new FromStringEnumConverter<StorageEngine>());
            context.RegisterConverter(new FromStringEnumConverter<FieldType>());
            context.RegisterConverter(new FromStringEnumConverter<IndexPartType>());
            context.RegisterConverter(new FromStringEnumConverter<IndexType>());

            context.RegisterConverter(new StringSliceOperationConverter());
            context.RegisterGenericConverter(typeof(UpdateOperationConverter<>));

            context.RegisterConverter(new ResponseHeaderConverter());
            context.RegisterConverter(new RequestHeaderConverter());

            context.RegisterConverter(new AuthenticationPacketConverter());
            context.RegisterConverter(new EmptyResponseConverter());
            context.RegisterConverter(new ErrorResponsePacketConverter());

            context.RegisterConverter(new SpaceFieldConverter());
            context.RegisterConverter(new SpaceConverter());
            context.RegisterConverter(new IndexPartConverter());
            context.RegisterConverter(new IndexCreationOptionsConverter());
            context.RegisterConverter(new IndexConverter());
            context.RegisterConverter(new TupleConverter());

            context.RegisterGenericConverter(typeof(ResponsePacketConverter<>));
            context.RegisterGenericConverter(typeof(UpdatePacketConverter<>));
            context.RegisterGenericConverter(typeof(CallPacketConverter<>));
            context.RegisterGenericConverter(typeof(DeletePacketConverter<>));
            context.RegisterGenericConverter(typeof(EvalPacketConverter<>));
            context.RegisterGenericConverter(typeof(InsertReplacePacketConverter<>));
            context.RegisterGenericConverter(typeof(SelectPacketConverter<>));
            context.RegisterGenericConverter(typeof(UpsertPacketConverter<>));
            context.RegisterConverter(new PingPacketConverter());

            context.RegisterGenericConverter(typeof(TupleConverter<>));
            context.RegisterGenericConverter(typeof(TupleConverter<,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,,,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,,,,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));
            context.RegisterGenericConverter(typeof(TupleConverter<,,,,,,,>));

            context.RegisterGenericConverter(typeof(SystemTupleConverter<>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,,>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,,,>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,,,,>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,,,,,>));
            context.RegisterGenericConverter(typeof(SystemTupleConverter<,,,,,,>));

            context.RegisterGenericConverter(typeof(ValueTupleConverter<>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,,>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,,,>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,,,,>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,,,,,>));
            context.RegisterGenericConverter(typeof(ValueTupleConverter<,,,,,,>));
        }
    }
}