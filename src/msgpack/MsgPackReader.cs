namespace TarantoolDnx.MsgPack
{
    //public class MsgPackReader : IMsgPackReader
    //{
    //    private readonly IBytesReader _reader;

    //    public MsgPackReader(Stream stream)
    //    {
    //        _reader = new BytesReader(stream);
    //    }

    //    public string ReadString()
    //    {
    //        var value = _reader.ReadByte();
    //        switch (value)
    //        {
    //            case DataTypes.Null:
    //                return null;
    //            case DataTypes.Str8:
    //                return ReadString(_reader.ReadByte() & IntLimits.Max8Bit);
    //            case DataTypes.Str16:
    //                return ReadString(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Str32:
    //                return ReadString(_reader.ReadInt32());
    //        }
    //        if (value >= DataTypes.FixStr && value <= DataTypes.FixStr + IntLimits.Max5Bit)
    //        {
    //            return ReadString(value - DataTypes.FixStr);
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public double ReadDouble()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Double)
    //        {
    //            return _reader.ReadDouble();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public float ReadFloat()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Single)
    //        {
    //            return _reader.ReadFloat();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public bool ReadBool()
    //    {
    //        var value = _reader.ReadByte();

    //        switch (value)
    //        {
    //            case DataTypes.False:
    //                return false;
    //            case DataTypes.True:
    //                return true;
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public byte ReadByte()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.UInt8)
    //        {
    //            return _reader.ReadByte();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public sbyte ReadSByte()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Int8)
    //        {
    //            return _reader.ReadSByte();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public short ReadShort()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Int16)
    //        {
    //            return _reader.ReadInt16();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public ushort ReadUShort()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.UInt16)
    //        {
    //            return _reader.ReadUInt16();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public int ReadInt()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Int32)
    //        {
    //            return _reader.ReadInt32();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public uint ReadUInt()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.UInt32)
    //        {
    //            return _reader.ReadUInt32();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public long ReadLong()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.Int64)
    //        {
    //            return _reader.ReadInt64();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public ulong ReadULong()
    //    {
    //        var value = _reader.ReadByte();
    //        if (value == DataTypes.UInt64)
    //        {
    //            return _reader.ReadUInt64();
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public byte[] ReadBinary()
    //    {
    //        var value = _reader.ReadByte();
    //        switch (value)
    //        {
    //            case DataTypes.Null:
    //                return null;
    //            case DataTypes.Bin8:
    //                return ReadBinary(_reader.ReadByte() & IntLimits.Max8Bit);
    //            case DataTypes.Bin16:
    //                return ReadBinary(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Bin32:
    //                return ReadBinary(_reader.ReadInt32());
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public T[] ReadArray<T>()
    //    {
    //        var value = _reader.ReadByte();
    //        switch (value)
    //        {
    //            case DataTypes.Null:
    //                return null;
    //            case DataTypes.Array16:
    //                return ReadArray<T>(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Array32:
    //                return ReadArray<T>(_reader.ReadInt32());
    //        }

    //        if (value >= DataTypes.FixArray && value <= DataTypes.FixArray + IntLimits.Max4Bit)
    //        {
    //            return ReadArray<T>(value - DataTypes.FixArray);
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }

    //    public IDictionary<TK, TV> ReadMap<TK, TV>()
    //    {
    //        var value = _reader.ReadByte();
    //        switch (value)
    //        {
    //            case DataTypes.Null:
    //                return null;
    //            case DataTypes.Map16:
    //                return ReadMap<TK, TV>(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Map32:
    //                return ReadMap<TK, TV>(_reader.ReadInt32());
    //        }

    //        if (value >= DataTypes.FixMap && value <= DataTypes.FixMap + IntLimits.Max4Bit)
    //        {
    //            return ReadMap<TK, TV>(value - DataTypes.FixMap);
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }
    

    //    private string ReadString(int size)
    //    {
    //        if (size < 0)
    //        {
    //            throw new ArgumentException("String to unpack too large (more than 2^31 elements)!");
    //        }

    //        var data = _reader.ReadBytes(size);

    //        return Encoding.UTF8.GetString(data, 0, data.Length);
    //    }

    //    private byte[] ReadBinary(int size)
    //    {
    //        if (size < 0)
    //        {
    //            throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
    //        }

    //        var data = _reader.ReadBytes(size);

    //        return data;
    //    }

    //    private T[] ReadArray<T>(int size)
    //    {
    //        if (size < 0)
    //        {
    //            throw new ArgumentException("Array to unpack too large (more than 2^31 elements)!");
    //        }
    //        var ret = new T[size];

    //        for (var i = 0; i < size; ++i)
    //        {
    //            ret[i] = (T)ReadObject();
    //        }

    //        return ret;
    //    }

    //    private IDictionary<TK, TV> ReadMap<TK, TV>(int size)
    //    {
    //        if (size < 0)
    //        {
    //            throw new ArgumentException("Map to unpack too large (more than 2^31 elements)!");
    //        }

    //        var ret = new Dictionary<TK, TV>(size);
    //        for (var i = 0; i < size; ++i)
    //        {
    //            var key = (TK)ReadObject();
    //            var value = (TV)ReadObject();
    //            ret.Add(key, value);
    //        }

    //        return ret;
    //    }

    //    private object ReadObject()
    //    {
    //        var value = _reader.ReadByte();

    //        switch (value)
    //        {
    //            case DataTypes.Null:
    //                return null;
    //            case DataTypes.False:
    //                return false;
    //            case DataTypes.True:
    //                return true;
    //            case DataTypes.Single:
    //                return _reader.ReadFloat();
    //            case DataTypes.Double:
    //                return _reader.ReadDouble();
    //            case DataTypes.UInt8:
    //                return _reader.ReadByte();
    //            case DataTypes.UInt16:
    //                return _reader.ReadUInt16();
    //            case DataTypes.UInt32:
    //                return _reader.ReadUInt32();
    //            case DataTypes.UInt64:
    //                return _reader.ReadUInt64();
    //            case DataTypes.Int8:
    //                return _reader.ReadSByte();
    //            case DataTypes.Int16:
    //                return _reader.ReadInt16();
    //            case DataTypes.Int32:
    //                return _reader.ReadInt32();
    //            case DataTypes.Int64:
    //                return _reader.ReadInt64();
    //            case DataTypes.Array16:
    //                return ReadArray<object>(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Array32:
    //                return ReadArray<object>(_reader.ReadInt32());
    //            case DataTypes.Map16:
    //                return ReadMap<object, object>(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Map32:
    //                return ReadMap<object, object>(_reader.ReadInt32());
    //            case DataTypes.Str8:
    //                return ReadString(_reader.ReadByte() & IntLimits.Max8Bit);
    //            case DataTypes.Str16:
    //                return ReadString(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Str32:
    //                return ReadString(_reader.ReadInt32());
    //            case DataTypes.Bin8:
    //                return ReadBinary(_reader.ReadByte() & IntLimits.Max8Bit);
    //            case DataTypes.Bin16:
    //                return ReadBinary(_reader.ReadInt16() & IntLimits.Max16Bit);
    //            case DataTypes.Bin32:
    //                return ReadBinary(_reader.ReadInt32());
    //        }

    //        if ((value & DataTypes.NegativeFixnum) == DataTypes.NegativeFixnum)
    //            return (sbyte)value;

    //        if (value >= DataTypes.NegativeFixnum && value <= DataTypes.NegativeFixnum + IntLimits.Max5Bit)
    //        {
    //            return value;
    //        }

    //        if (value >= DataTypes.FixArray && value <= DataTypes.FixArray + IntLimits.Max4Bit)
    //        {
    //            return ReadArray<object>(value - DataTypes.FixArray);
    //        }

    //        if (value >= DataTypes.FixMap && value <= DataTypes.FixMap + IntLimits.Max4Bit)
    //        {
    //            return ReadMap<object, object>(value - DataTypes.FixMap);
    //        }

    //        if (value >= DataTypes.FixStr && value <= DataTypes.FixStr + IntLimits.Max5Bit)
    //        {
    //            return ReadString(value - DataTypes.FixStr);
    //        }

    //        if (value <= IntLimits.Max7Bit)
    //        {
    //            //MP_FIXNUM - the value is value as an int
    //            return value;
    //        }

    //        throw new ArgumentException("Input contains invalid type value " + value);
    //    }
    //}
}