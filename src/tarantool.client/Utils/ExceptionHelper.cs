using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;

namespace Tarantool.Client
{
    public static class ExceptionHelper
    {
        public static Exception UnexpectedGreetingBytesCount(int readCount)
        {
            return new ArgumentException($"Invalid greetings response length. 128 is expected, but got {readCount}.");
        }
        
        public static Exception InvalidMapLength(uint expected, uint? actual)
        {
            return new ArgumentException($"Invalid map length: {expected} is expected, but got {actual}.");
        }

        public static Exception UnexpectedKey(Key expected, Key actual)
        {
            return new ArgumentException($"Unexpected key: {expected} is expected, but got {actual}.");
        }

        public static Exception InvalidArrayLength(uint expected, uint? actual)
        {
            return new ArgumentException($"Invalid array length: {expected} is expected, but got {actual}.");
        }

        public static Exception UnexpectedDataType(DataTypes expected, DataTypes actual)
        {
            return new ArgumentException($"Unexpected data type: {expected} is expected, but got {actual}.");
        }
    }
}