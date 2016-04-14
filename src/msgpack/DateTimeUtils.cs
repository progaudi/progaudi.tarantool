using System;

namespace TarantoolDnx.MsgPack
{
    public static class DateTimeUtils
    {
        private static readonly DateTime UnixEpocUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTimeOffset ToDateTimeOffset(long value)
        {
            return UnixEpocUtc.AddTicks(value);
        }

        public static long FromDateTimeOffset(DateTimeOffset value)
        {
            return value.ToUniversalTime().Subtract(UnixEpocUtc).Ticks;
        }

        public static DateTime ToDateTime(long value)
        {
            return UnixEpocUtc.AddTicks(value);
        }

        public static long FromDateTime(DateTime value)
        {
            return value.ToUniversalTime().Subtract(UnixEpocUtc).Ticks;
        }
    }
}