using System;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    public static class ExceptionUtils
    {
        public static Exception BadTypeException(DataTypes actual, params DataTypes[] expectedCodes)
        {
            return new SerializationException($"Got {actual:G} (0x{actual:X}), while expecting one of these: {string.Join(", ", expectedCodes)}");
        }

        public static Exception NotEnoughBytes(int actual, int expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception CantReadReadOnlyCollection(Type type)
        {
            return new SerializationException($"Can't deserialize into read-only collection {type.Name}. Create a specialized converter for that.");
        }
    }
}
