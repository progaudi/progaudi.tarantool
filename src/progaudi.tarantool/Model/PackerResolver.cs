using System;
using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client.Model
{
    public sealed class PackerResolver : IFormatterResolver
    {
        public static readonly PackerResolver Instance = new PackerResolver();

        private PackerResolver()
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

                if (type == typeof(DataResponse))
                {
                    Formatter = new DataResponse.Formatter() as IMessagePackFormatter<T>;
                    return;
                }

                if (!type.IsGenericType) return;

                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(CallRequest<>))
                {
                    var result = typeof(CallRequest<>.Formatter).MakeGenericType(type.GenericTypeArguments);
                    Formatter = (IMessagePackFormatter<T>) Activator.CreateInstance(result);
                    return;
                }

                if (
                    genericTypeDefinition == typeof(InsertReplaceRequest<>) || 
                    genericTypeDefinition == typeof(ReplaceRequest<>) || 
                    genericTypeDefinition == typeof(InsertRequest<>)
                    )
                {
                    var result = typeof(InsertReplaceRequest<>.Formatter).MakeGenericType(type.GenericTypeArguments);
                    Formatter = (IMessagePackFormatter<T>) Activator.CreateInstance(result);
                    return;
                }

                if (genericTypeDefinition == typeof(SelectRequest<>))
                {
                    var result = typeof(SelectRequest<>.Formatter).MakeGenericType(type.GenericTypeArguments);
                    Formatter = (IMessagePackFormatter<T>) Activator.CreateInstance(result);
                    return;
                }

                if (genericTypeDefinition == typeof(DataResponse<>))
                {
                    var result = typeof(DataResponse<>.Formatter).MakeGenericType(type.GenericTypeArguments);
                    Formatter = (IMessagePackFormatter<T>) Activator.CreateInstance(result);
                }
            }
        }
    }
}