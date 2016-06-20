using System;

using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client.IProto.Converters
{
    public class AuthenticationResponseConverter : IMsgPackConverter<AuthenticationResponse>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(AuthenticationResponse value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public AuthenticationResponse Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();
            length.Value.ShouldBe((uint)0);

            return new AuthenticationResponse();
        }
    }
}