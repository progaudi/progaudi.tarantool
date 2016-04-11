using System;
using System.IO;
// ReSharper disable once RedundantUsingDirective
using System.Reflection;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack.Tests
{
    public class TestReflectionConverter : IMsgPackConverter<object>
    {
        public void Write(object value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            var converter = GetConverter(value, settings);
            if (converter == null)
                throw new SerializationException($"Please, provide convertor for {value.GetType().Name}");

            var methodDefinition = converter.GetType().GetMethod(
                "Write",
                new[] {value.GetType(), typeof(Stream), typeof(MsgPackSettings)});

            methodDefinition.Invoke(converter, new[] {value, stream, settings});
        }

        public object Read(Stream stream, MsgPackSettings settings, Func<object> creator)
        {
            throw new System.NotImplementedException();
        }

        private static object GetConverter(object value, MsgPackSettings settings)
        {
            var methodDefinition = typeof(MsgPackSettings).GetMethod(nameof(MsgPackSettings.GetConverter));
            var concreteMethod = methodDefinition.MakeGenericMethod(value.GetType());
            var converter = concreteMethod.Invoke(settings, null);
            return converter;
        }
    }
}