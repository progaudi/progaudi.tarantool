using JetBrains.Annotations;
using System;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackConverter
    {
    }

    public interface IMsgPackConverter<T> : IMsgPackConverter
    {
        void Write([CanBeNull] T value, [NotNull] IBytesWriter writer, [NotNull] MsgPackContext context);

        T Read([NotNull] IBytesReader reader, [NotNull] MsgPackContext context, Func<T> creator);
    }
}