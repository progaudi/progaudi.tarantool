using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Converters;
using ProGaudi.Tarantool.Client.Formatters;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client
{
    public static class TarantoolConvertersRegistrator
    {
        public static void Register(MsgPackContext context)
        {
            context.GetFormatter<IndexType>();
            
            context.RegisterFormatter(new EnumFormatter<Key>());
            context.RegisterFormatter(new EnumFormatter<CommandCode>());
            context.RegisterFormatter(new EnumFormatter<Iterator>());
            context.RegisterGenericFormatter(typeof(CallPacketFormatter<>));
            context.RegisterGenericFormatter(typeof(EvalPacketFormatter<>));
            context.RegisterGenericFormatter(typeof(DeletePacketFormatter<>));
            context.RegisterGenericFormatter(typeof(InsertReplacePacketFormatter<>));
            context.RegisterGenericFormatter(typeof(SelectPacketFormatter<>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,,,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,,,,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,,,,,>));
            context.RegisterGenericFormatter(typeof(ValueTupleFormatter<,,,,,,,>));
            
            context.RegisterFormatter(new PingPacketFormatter());
            context.RegisterFormatter(new AuthenticationPacketFormatter(context));
            context.RegisterFormatter(new RequestHeaderFormatter(context));

            context.RegisterParser(new EnumFormatter<Key>());
            context.RegisterParser(new EnumFormatter<CommandCode>());
            context.RegisterParser(new EnumFormatter<Iterator>());
            context.RegisterGenericSequenceParser(typeof(ResponsePacketParser<>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,,,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,,,,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,,,,,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,>));
            context.RegisterGenericSequenceParser(typeof(ValueTupleParser<,,,,,,,>));

            context.RegisterParser(new ResponseHeaderParser(context));
            context.RegisterParser(new EmptyResponseParser(context));
            context.RegisterParser(new ErrorResponsePacketParser(context));
            context.RegisterParser(new SpaceFieldParser());
            context.RegisterParser(new SpaceParser(context));
            context.RegisterParser(new IndexPartParser());
            context.RegisterParser(new IndexCreationOptionsParser());
            context.RegisterParser(new IndexParser(context));
            context.RegisterParser(new BoxInfo.Converter());
            context.RegisterParser(new ResponsePacketParser(context));
        }
    }
}