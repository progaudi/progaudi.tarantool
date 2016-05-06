using System;

using Shouldly;

using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class ResponsePacketConverter<T> : IMsgPackConverter<ResponsePacket<T>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(ResponsePacket<T> value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ResponsePacket<T> Read(IMsgPackReader reader)
        {
            var headerConverter = _context.GetConverter<Header>();
            var keyConverter = _context.GetConverter<Key>();

            var header = headerConverter.Read(reader);
            string errorMessage = null;
            T data = default(T);

            var length = reader.ReadMapLength();

            length.HasValue.ShouldBeTrue();

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                length.ShouldBe(1u);

                var stringConverter = _context.GetConverter<string>();

                keyConverter.Read(reader).ShouldBe(Key.Error);
                errorMessage = stringConverter.Read(reader);
            }
            else if (length.Value > 0u)
            {
                var dataConverter = _context.GetConverter<T>();

                keyConverter.Read(reader).ShouldBe(Key.Data);
                data = dataConverter.Read(reader);
            }

            return new ResponsePacket<T>(header, errorMessage, data);
        }
    }
}