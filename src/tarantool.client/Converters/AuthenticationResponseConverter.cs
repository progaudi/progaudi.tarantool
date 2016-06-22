using System;

using MsgPack.Light;

using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
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
            
            if (length != 0)
            {
                throw ExceptionHelper.InvalidMapLength(0, length);
            }

            return new AuthenticationResponse();
        }
    }
}