using System;

using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class ErrorResponsePacketConverter : IMsgPackConverter<ErrorResponsePacket>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
        }

        public void Write(ErrorResponsePacket value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ErrorResponsePacket Read(IMsgPackReader reader)
        {
            string errorMessage = null;
            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            length.ShouldBe(1u);

            _keyConverter.Read(reader).ShouldBe(Key.Error);
            errorMessage = _stringConverter.Read(reader);

            return new ErrorResponsePacket(errorMessage);
        }
    }
}