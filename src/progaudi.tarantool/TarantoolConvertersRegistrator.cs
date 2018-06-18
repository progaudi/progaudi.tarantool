using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Converters;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public class TarantoolConvertersRegistrator
    {
        public static void Register(MsgPackContext context)
        {
            context.DiscoverConverters<Box>();
            
            context.RegisterConverter(new EnumConverter<CommandCodes>());
            context.RegisterConverter(new EnumConverter<Iterator>());

            context.RegisterConverter(new EnumAsStringFormatter<StorageEngine>());
            context.RegisterConverter(new EnumAsStringFormatter<FieldType>());
            context.RegisterConverter(new EnumAsStringFormatter<IndexType>());

            //context.RegisterConverter(new StringSliceOperationConverter());
            //context.RegisterGenericConverter(typeof(UpdateOperationConverter<>));

            context.RegisterConverter(new ResponseHeader.Formatter());
            context.RegisterConverter(new RequestHeader.Formatter());

            context.RegisterConverter(new AuthenticationRequest.Formatter());
            context.RegisterConverter(new IndexPart.Formatter());
            //context.RegisterConverter(new ErrorResponse);

            //context.RegisterConverter(new TupleConverter());
            //context.RegisterConverter(new BoxInfo.Converter());

            context.RegisterGenericConverter(typeof(DataResponse<>.Formatter));
            context.RegisterConverter(new DataResponse.Formatter());
            //context.RegisterGenericConverter(typeof(UpdatePacketConverter<>));
            //context.RegisterGenericConverter(typeof(CallPacketConverter<>));
            //context.RegisterGenericConverter(typeof(DeletePacketConverter<>));
            //context.RegisterGenericConverter(typeof(EvalPacketConverter<>));
            context.RegisterGenericConverter(typeof(InsertReplaceRequest<>.Formatter));
            context.RegisterGenericConverter(typeof(SelectRequest<>.Formatter));
            //context.RegisterGenericConverter(typeof(UpsertPacketConverter<>));
            //context.RegisterConverter(new PingPacketConverter());
            //context.RegisterConverter(new ExecuteSqlRequestConverter());

            //context.RegisterGenericConverter(typeof(TupleConverter<>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,,,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,,,,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,,,,,>));
            //context.RegisterGenericConverter(typeof(TupleConverter<,,,,,,,>));

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