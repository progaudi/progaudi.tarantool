using System;

namespace TarantoolDnx.MsgPack
{
    internal sealed class ByteUtils
    {
        public static byte[] ReverseArrayIfNeeded(byte[] array)
        {
            if (!BitConverter.IsLittleEndian)
                return array;

            var result = new byte[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                result[array.Length - 1 - i] = array[i];
            }

            return result;
        }
    }
}