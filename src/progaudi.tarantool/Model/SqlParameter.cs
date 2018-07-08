using System;

namespace ProGaudi.Tarantool.Client.Model
{
    //public abstract class SqlParameter
    //{
    //    internal abstract void Write(MsgPackContext context, IMsgPackWriter writer, IMsgPackConverter<string> stringConverter);
    //}

    //public class SqlParameter<T> : SqlParameter
    //{
    //    private readonly T _value;
    //    private readonly string _name;

    //    public SqlParameter(T value) => _value = value;

    //    public SqlParameter(T value, string name)
    //        : this(value)
    //    {
    //        if (name[0] != ':' && name[0] != '@' && name[0] != '$') throw new ArgumentException("Name should start either with ':', '$' or '@'.", nameof(name));

    //        _name = name;
    //    }

    //    internal override void Write(MsgPackContext context, IMsgPackWriter writer, IMsgPackConverter<string> stringConverter)
    //    {
    //        if (_name != null)
    //        {
    //            writer.WriteMapHeader(1u);
    //            stringConverter.Write(_name, writer);
    //        }

    //        context.GetConverter<T>().Write(_value, writer);
    //    }
    //}
}
