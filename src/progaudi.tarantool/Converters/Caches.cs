using System;
using System.Collections.Generic;
using ProGaudi.MsgPack;

namespace ProGaudi.Tarantool.Client.Formatters
{
    internal static class Caches
    {
        public static readonly Dictionary<Type, int> Lengths = new Dictionary<Type, int>
        {
            { typeof(byte), DataLengths.UInt8 },
            { typeof(sbyte), DataLengths.Int8 },
            { typeof(short), DataLengths.Int16 },
            { typeof(ushort), DataLengths.UInt16 },
            { typeof(int), DataLengths.Int32 },
            { typeof(uint), DataLengths.UInt32 },
            { typeof(long), DataLengths.Int64 },
            { typeof(ulong), DataLengths.UInt64 },
        };
    }
}