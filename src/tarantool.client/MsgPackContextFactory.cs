using MsgPack.Light;

using Tarantool.Client.IProto.Converters;
using Tarantool.Client.IProto.Data;

namespace Tarantool.Client
{
    public class MsgPackContextFactory
    {
        public static MsgPackContext Create()
        {
            var result = new MsgPackContext();

            result.RegisterConverter(new EnumConverter<Key>());
            result.RegisterConverter(new EnumConverter<CommandCode>());
            result.RegisterConverter(new EnumConverter<Iterator>());
            result.RegisterConverter(new RequestIdConverter());
            result.RegisterConverter(new PacketSizeConverter());

            result.RegisterConverter(new FromStringEnumConverter<StorageEngine>());
            result.RegisterConverter(new FromStringEnumConverter<FieldType>());
            result.RegisterConverter(new FromStringEnumConverter<IndexPartType>());
            result.RegisterConverter(new FromStringEnumConverter<IndexType>());

            result.RegisterConverter(new StringSliceOperationConverter());
            result.RegisterGenericConverter(typeof(UpdateOperationConverter<>));

            result.RegisterConverter(new ResponseHeaderConverter());
            result.RegisterConverter(new RequestHeaderConverter());

            result.RegisterConverter(new AuthenticationPacketConverter());
            result.RegisterConverter(new AuthenticationResponseConverter());
            result.RegisterConverter(new ErrorResponsePacketConverter());

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