using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Utils;
using System;
using System.Buffers.Binary;

namespace ProGaudi.Tarantool.Client.Converters
{
    /// <summary>
    /// Converter for Tarantool datetime values, implemeted as MsgPack extension.
    /// See https://www.tarantool.io/ru/doc/latest/dev_guide/internals/msgpack_extensions/#the-datetime-type
    /// </summary>
    internal class DateTimeConverter : IMsgPackConverter<DateTime>, IMsgPackConverter<DateTimeOffset>
    {
        private const byte MP_DATETIME = 0x04;
        private static readonly DateTime UnixEpocUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void Initialize(MsgPackContext context)
        {
        }

        public DateTime Read(IMsgPackReader reader)
        {
            var dataType = reader.ReadByte();
            var mpHeader = reader.ReadByte();
            if (mpHeader != MP_DATETIME)
            {
                throw ExceptionHelper.UnexpectedMsgPackHeader(mpHeader, MP_DATETIME);
            }

            if (dataType == MsgPackExtDataTypes.FixExt8)
            {
                var seconds = BinaryPrimitives.ReadInt32LittleEndian(reader.ReadBytes(4));
                var nanoSeconds = BinaryPrimitives.ReadInt16LittleEndian(reader.ReadBytes(2));
                var _ = reader.ReadBytes(2);// also need to extract tzoffset; tzindex;
                return UnixEpocUtc.AddSeconds(seconds).AddTicks(nanoSeconds / 100);  
            }
            else if (dataType == MsgPackExtDataTypes.FixExt16)
            {
                var seconds = BinaryPrimitives.ReadInt64LittleEndian(reader.ReadBytes(8));
                var nanoSeconds = BinaryPrimitives.ReadInt32LittleEndian(reader.ReadBytes(4));
                var _ = reader.ReadBytes(4);// also need to extract tzoffset; tzindex;
                return UnixEpocUtc.AddSeconds(seconds).AddTicks(nanoSeconds / 100);
            }

            throw ExceptionHelper.UnexpectedDataType(dataType, MsgPackExtDataTypes.FixExt8, MsgPackExtDataTypes.FixExt16);
        }

        DateTimeOffset IMsgPackConverter<DateTimeOffset>.Read(IMsgPackReader reader)
        {
            return Read(reader);
        }

        public void Write(DateTimeOffset value, IMsgPackWriter writer)
        {
            var timeSpan = value.ToUniversalTime().Subtract(UnixEpocUtc);
            long seconds = (long)timeSpan.TotalSeconds;
            timeSpan = timeSpan.Subtract(TimeSpan.FromSeconds(seconds));
            int nanoSeconds = (int)(timeSpan.Ticks * 100);
            int _ = 0;// also need to extract tzoffset; tzindex;

            writer.Write(MsgPackExtDataTypes.FixExt16);
            writer.Write(MP_DATETIME);

            var byteArray = new byte[8];
            var span = new Span<byte>(byteArray);
            BinaryPrimitives.WriteInt64LittleEndian(span, seconds);
            writer.Write(byteArray);

            byteArray = new byte[4];
            span = new Span<byte>(byteArray);
            BinaryPrimitives.WriteInt32LittleEndian(span, nanoSeconds);
            writer.Write(byteArray);

            byteArray = new byte[4];
            span = new Span<byte>(byteArray);
            BinaryPrimitives.WriteInt32LittleEndian(span, _);
            writer.Write(byteArray);
            
        }

        public void Write(DateTime value, IMsgPackWriter writer)
        {
            Write((DateTimeOffset)value, writer);
        }
    }
}
