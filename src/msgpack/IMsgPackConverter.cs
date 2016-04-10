using System.IO;
using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackConverter
    {
    }

    public interface IMsgPackConverter<in T> : IMsgPackConverter
    {
        void Write([CanBeNull]T value, [NotNull]Stream stream, [NotNull]MsgPackSettings settings);
    }
}