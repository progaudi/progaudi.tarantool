using System;

using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class AuthenticationResponseConverter : IMsgPackConverter<AuthenticationResponse>
    {
        private IMsgPackConverter<Key> _keyConverter;

        private IMsgPackConverter<string> _stringConverter;

        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _stringConverter = context.GetConverter<string>();
            _nullConverter = context.NullConverter;
        }

        public void Write(AuthenticationResponse value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public AuthenticationResponse Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            _keyConverter.Read(reader).ShouldBe(Key.Data);
            _nullConverter.Read(reader);

            return new AuthenticationResponse();
        }
    }
}