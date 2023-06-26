using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Utils;
using System;
using System.Buffers.Binary;
using System.Linq;

namespace ProGaudi.Tarantool.Client.Converters
{
    /// <summary>
    /// Converter for Tarantool uuid values, implemeted as MsgPack extension.
    /// See https://www.tarantool.io/ru/doc/latest/dev_guide/internals/msgpack_extensions/#the-uuid-type
    /// </summary>
    internal class GuidConverter : IMsgPackConverter<Guid>
    {
        private static readonly byte GuidDataType = MsgPackExtDataTypes.FixExt16;
        private const byte MP_UUID = 0x02;

        public void Initialize(MsgPackContext context)
        {
        }

        public Guid Read(IMsgPackReader reader)
        {
            var dataType = reader.ReadByte();
            if (dataType != GuidDataType)
            {
                throw ExceptionHelper.UnexpectedDataType(dataType, GuidDataType);
            }

            var mpHeader = reader.ReadByte();
            if (mpHeader != MP_UUID)
            {
                throw ExceptionHelper.UnexpectedMsgPackHeader(mpHeader, MP_UUID);
            }

            int intToken = BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(4));
            short shortToken1 = BinaryPrimitives.ReadInt16BigEndian(reader.ReadBytes(2));
            short shortToken2 = BinaryPrimitives.ReadInt16BigEndian(reader.ReadBytes(2));

            return new Guid(intToken, shortToken1, shortToken2, reader.ReadBytes(8).ToArray());
        }

        public void Write(Guid value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
