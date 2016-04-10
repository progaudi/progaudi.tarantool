using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    public class MsgPackWriter
    {
        private static readonly HashSet<Type> AllowedTypes = new HashSet<Type>
        {
            typeof(bool),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(string),
            typeof(IReadOnlyList<byte>)
        };

        private static readonly ConcurrentDictionary<Type, bool> CheckedTypes = new ConcurrentDictionary<Type, bool>();

        private readonly MemoryStream stream = new MemoryStream();

        private readonly MsgPackSettings settings = new MsgPackSettings();

        public void Write<T>(IReadOnlyList<T> data)
        {
            if (!IsTypeSerializable(typeof(IReadOnlyList<T>)))
            {
                throw new SerializationException($"Can't serialize IReadOnlyList<{typeof(T).Name}>");
            }

            WriteArrayHeaderAndLength(data.Count);

            Write((IReadOnlyList<int>)data);
        }

        public void Write<TK, TV>(IReadOnlyDictionary<TK, TV> map)
        {
            if (!IsTypeSerializable(typeof(IReadOnlyDictionary<TK, TV>)))
            {
                throw new SerializationException($"Can't serialize IReadOnlyDictionary<{typeof(TK).Name}, {typeof(TV).Name}>");
            }

            WriteMapHeaderAndLength(map.Count);
        }

        private static bool IsTypeSerializable(Type type)
        {
            if (AllowedTypes.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            bool allowed;
            if (CheckedTypes.TryGetValue(type, out allowed))
            {
                return allowed;
            }

            var typeInfo = type.GetTypeInfo();
            var arrayInterface = typeInfo.ImplementedInterfaces
                .Select(x => x.GetTypeInfo())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReadOnlyList<>));

            if (arrayInterface != null)
            {
                var elementType = arrayInterface.GenericTypeArguments[0];
                var result = IsTypeSerializable(elementType);

                CheckedTypes.TryAdd(elementType, result);
                CheckedTypes.TryAdd(type, result);

                return result;
            }

            var mapInterface = typeInfo.ImplementedInterfaces
                .Select(x => x.GetTypeInfo())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));

            if (mapInterface != null)
            {
                var keyType = mapInterface.GenericTypeParameters[0];
                var valueType = mapInterface.GenericTypeParameters[1];
                var isKeySerializable = IsTypeSerializable(keyType);
                var isValueSerializable = IsTypeSerializable(valueType);
                var result = isKeySerializable && isValueSerializable;

                CheckedTypes.TryAdd(keyType, isKeySerializable);
                CheckedTypes.TryAdd(valueType, isValueSerializable);
                CheckedTypes.TryAdd(type, result);
            }

            var parent = typeInfo.BaseType;
            if (parent == null)
            {
                CheckedTypes.TryAdd(type, false);
                return false;
            }

            var isParentSerializable = IsTypeSerializable(parent);
            CheckedTypes.TryAdd(parent, isParentSerializable);
            CheckedTypes.TryAdd(type, isParentSerializable);

            return isParentSerializable;
        }

        #region Headers

        private void WriteArrayHeaderAndLength(int length)
        {
            WriteHeaderAndLength(length, DataTypes.FixArray, DataTypes.Array16, DataTypes.Array32);
        }

        private void WriteMapHeaderAndLength(int length)
        {
            WriteHeaderAndLength(length, DataTypes.FixMap, DataTypes.Map16, DataTypes.Map32);
        }

        private void WriteHeaderAndLength(int length, DataTypes lessThan15, DataTypes length16Bit, DataTypes length32Bit)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte)((byte)lessThan15 + length), stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)length16Bit);
                IntConverter.WriteValue((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)length32Bit);
                IntConverter.WriteValue((uint)length, stream);
            }
        }

        #endregion
    }
}