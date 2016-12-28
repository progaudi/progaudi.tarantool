using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class PingPacketConverter : IMsgPackConverter<PingRequest>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(PingRequest value, IMsgPackWriter writer)
        {
        }

        public PingRequest Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}