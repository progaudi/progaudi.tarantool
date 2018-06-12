using System;
using System.Reflection;
using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client.Model
{
    public sealed class EnumResolver : IFormatterResolver
    {
        public static readonly EnumResolver Instance = new EnumResolver();

        private EnumResolver()
        {
        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return Cache<T>.Formatter;
        }

        private static class Cache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            static Cache()
            {
                var type = typeof(T);
                if (!type.IsEnum)
                {
                    return;
                }

                var enumAsString = type.GetCustomAttribute<EnumAsStringAttribute>();
                if (enumAsString == null)
                {
                    return;
                }

                Formatter = (IMessagePackFormatter<T>) Activator.CreateInstance(typeof(EnumAsStringFormatter<>).MakeGenericType(type), new object[] { enumAsString.IgnoreCase });
            }
        }
    }
}