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
            context.GetFormatter<StorageEngine>();
            context.GetFormatter<FieldType>();
            context.GetFormatter<IndexPartType>();
            context.GetFormatter<IndexType>();
            
            context.RegisterFormatter(new EnumFormatter<Key>());
            context.RegisterFormatter(new EnumFormatter<CommandCode>());
            context.RegisterFormatter(new EnumFormatter<Iterator>());
            context.RegisterFormatter(new PingPacketConverter());
            context.RegisterFormatter(new AuthenticationPacketFormatter(context));
            context.RegisterFormatter(new RequestHeaderConverter(context));

            context.RegisterParser(new EnumFormatter<Key>());
            context.RegisterParser(new EnumFormatter<CommandCode>());
            context.RegisterParser(new EnumFormatter<Iterator>());

            context.RegisterParser(new ResponseHeaderConverter(context));
            context.RegisterParser(new EmptyResponseParser(context));
            context.RegisterParser(new ErrorResponsePacketParser(context));
            context.RegisterParser(new SpaceFieldConverter(context));
            context.RegisterParser(new SpaceConverter(context));
            context.RegisterParser(new IndexPartConverter(context));
            context.RegisterParser(new IndexCreationOptionsConverter());
            context.RegisterParser(new IndexConverter(context));
            context.RegisterParser(new BoxInfo.Converter());
            context.RegisterParser(new ResponsePacketConverter(context));
        }
    }
}