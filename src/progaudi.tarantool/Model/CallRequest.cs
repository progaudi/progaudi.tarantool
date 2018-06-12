using System;
using MessagePack;
using MessagePack.Formatters;
using static MessagePack.MessagePackBinary;

namespace ProGaudi.Tarantool.Client.Model
{
    public class CallRequest<T> : IRequest
    {
        private readonly bool _use17;

        public CallRequest(string functionName, T tuple, bool use17 = true)
        {
            _use17 = use17;
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        public CommandCodes Code => _use17 ? CommandCodes.Call : CommandCodes.OldCall;

        internal class Formatter : IMessagePackFormatter<CallRequest<T>>
        {
            public int Serialize(ref byte[] bytes, int offset, CallRequest<T> value, IFormatterResolver formatterResolver)
            {
                var startOffset = offset;

                offset += WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
                offset += WriteUInt32(ref bytes, offset, Keys.FunctionName);
                offset += WriteString(ref bytes, offset, value.FunctionName);
                offset += WriteUInt32(ref bytes, offset, Keys.Tuple);
                offset += formatterResolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Tuple, formatterResolver);

                return offset - startOffset;
            }

            public CallRequest<T> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new NotImplementedException();
            }
        }
    }
}
