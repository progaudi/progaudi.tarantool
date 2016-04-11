using System;
using System.IO;

using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackConverter
    {
    }

    public interface IMsgPackConverter<T> : IMsgPackConverter
    {
        void Write([CanBeNull] T value, [NotNull] Stream stream, [NotNull] MsgPackSettings settings);

        T Read([NotNull] Stream stream, [NotNull] MsgPackSettings settings, Func<T> creator);
    }
}