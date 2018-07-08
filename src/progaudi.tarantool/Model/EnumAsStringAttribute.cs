using System;

namespace ProGaudi.Tarantool.Client.Model
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class EnumAsStringAttribute : Attribute
    {
        public bool IgnoreCase { get; }

        public EnumAsStringAttribute(bool ignoreCase = false)
        {
            IgnoreCase = ignoreCase;
        }
    }
}