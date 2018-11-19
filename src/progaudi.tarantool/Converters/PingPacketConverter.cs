using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class PingPacketConverter : IMsgPackFormatter<PingRequest>
    {
        public int GetBufferSize(PingRequest value) => 0;

        public bool HasConstantSize => false;
        
        public int Format(Span<byte> destination, PingRequest value) => 0;
    }
}