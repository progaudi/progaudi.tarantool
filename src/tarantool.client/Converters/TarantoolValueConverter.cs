using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using System;
using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client.Converters
{
    public class TarantoolValueConverter : IMsgPackConverter<TarantoolValue>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(TarantoolValue value, IMsgPackWriter writer)
        {
            writer.Write((byte[])value);
        }

        public TarantoolValue Read(IMsgPackReader reader)
        {
            var bytesResult = new List<byte>();
            ReadTokenBytes(reader, bytesResult);
            return new TarantoolValue(bytesResult.ToArray());
        }

        private static void ReadTokenBytes(IMsgPackReader reader, List<byte> bytesResult)
        {
            var firstByte = reader.ReadDataType();
            bytesResult.Add((byte)firstByte);

            switch (firstByte)
            {
                case DataTypes.Null:
                case DataTypes.False:
                case DataTypes.True:
                    return;
                case DataTypes.UInt8:
                case DataTypes.Int8:
                    ReadBytes(reader, bytesResult,  1);
                    return;
                case DataTypes.UInt16:
                case DataTypes.Int16:
                    ReadBytes(reader, bytesResult,  2);
                    return;
                case DataTypes.UInt32:
                case DataTypes.Int32:
                case DataTypes.Single:
                    ReadBytes(reader, bytesResult,  4);
                    return;
                case DataTypes.UInt64:
                case DataTypes.Int64:
                case DataTypes.Double:
                    ReadBytes(reader, bytesResult,  8);
                    return;
                case DataTypes.Array16:
                    ReadArrayItems(reader, bytesResult, ReadUInt16(reader));
                    return;
                case DataTypes.Array32:
                    ReadArrayItems(reader, bytesResult, ReadUInt32(reader));
                    return;
                case DataTypes.Map16:
                    ReadMapItems(reader, bytesResult, ReadUInt16(reader));
                    return;
                case DataTypes.Map32:
                    ReadMapItems(reader, bytesResult, ReadUInt32(reader));
                    return;
                case DataTypes.Str8:
                    ReadBytes(reader, bytesResult,  ReadUInt8(reader));
                    return;
                case DataTypes.Str16:
                    ReadBytes(reader, bytesResult,  ReadUInt16(reader));
                    return;
                case DataTypes.Str32:
                    ReadBytes(reader, bytesResult,  ReadUInt32(reader));
                    return;
                case DataTypes.Bin8:
                    ReadBytes(reader, bytesResult,  ReadUInt8(reader));
                    return;
                case DataTypes.Bin16:
                    ReadBytes(reader, bytesResult,  ReadUInt16(reader));
                    return;
                case DataTypes.Bin32:
                    ReadBytes(reader, bytesResult,  ReadUInt32(reader));
                    return;
            }

            if (firstByte.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1) ||
                firstByte.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3))
            {
                return;
            }

            var arrayLength = TryGetLengthFromFixArray(firstByte);
            if (arrayLength.HasValue)
            {
                ReadArrayItems(reader, bytesResult, arrayLength.Value);
                return;
            }

            var mapLength = TryGetLengthFromFixMap(firstByte);
            if (mapLength.HasValue)
            {
                ReadMapItems(reader, bytesResult, mapLength.Value);
                return;
            }

            var stringLength = TryGetLengthFromFixStr(firstByte);
            if (stringLength.HasValue)
            {
                ReadBytes(reader, bytesResult,  stringLength.Value);
                return;
            }

            throw new ArgumentOutOfRangeException();
        }

        private static uint? TryGetLengthFromFixStr(DataTypes type)
        {
            var length = type - DataTypes.FixStr;
            return type.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixArray(DataTypes type)
        {
            var length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixMap(DataTypes type)
        {
            var length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4) ? length : (uint?)null;
        }

        private static void ReadMapItems(IMsgPackReader reader, List<byte> resultBytes, uint count)
        {
            for (var i = 0; i < count; i++)
            {
                ReadTokenBytes(reader, resultBytes);
                ReadTokenBytes(reader, resultBytes);
            }
        }

        private static void ReadArrayItems(IMsgPackReader reader, List<byte> resultBytes, uint count)
        {
            for (var i = 0; i < count; i++)
            {
                ReadTokenBytes(reader, resultBytes);
            }
        }
        private static void ReadBytes(IMsgPackReader reader,List<byte> resultBytes, uint bytesCount)
        {
            resultBytes.AddRange(reader.ReadBytes(bytesCount).Array);
        }

        private static byte ReadUInt8(IMsgPackReader reader)
        {
            return reader.ReadByte();
        }

        private static ushort ReadUInt16(IMsgPackReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) + reader.ReadByte());
        }

        private static uint ReadUInt32(IMsgPackReader reader)
        {
            var temp = (uint)(reader.ReadByte() << 24);
            temp += (uint)reader.ReadByte() << 16;
            temp += (uint)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }


    }
}
