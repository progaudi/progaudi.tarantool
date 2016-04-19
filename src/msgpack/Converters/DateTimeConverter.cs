using System;

namespace TarantoolDnx.MsgPack.Converters
{
    public class DateTimeConverter : IMsgPackConverter<DateTime>, IMsgPackConverter<DateTimeOffset>
    {
        public void Write(DateTime value, IBytesWriter writer, MsgPackContext context)
        {
            var longValue = DateTimeUtils.FromDateTime(value);
            var longConverter = context.GetConverter<long>();

            longConverter.Write(longValue, writer, context);
        }

        public DateTime Read(IBytesReader reader, MsgPackContext context, Func<DateTime> creator)
        {
            var longConverter = context.GetConverter<long>();
            var longValue = longConverter.Read(reader, context, null);
            return DateTimeUtils.ToDateTime(longValue);
        }

        public void Write(DateTimeOffset value, IBytesWriter writer, MsgPackContext context)
        {
            var longValue = DateTimeUtils.FromDateTimeOffset(value);
            var longConverter = context.GetConverter<long>();

            longConverter.Write(longValue, writer, context);
        }

        public DateTimeOffset Read(IBytesReader reader, MsgPackContext context, Func<DateTimeOffset> creator)
        {
            var longConverter = context.GetConverter<long>();
            var longValue = longConverter.Read(reader, context, null);
            return DateTimeUtils.ToDateTimeOffset(longValue);
        }
    }
}