using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class CallRequest<T> : Request
    {
        private readonly IMsgPackFormatter<CallRequest<T>> _formatter;

        public CallRequest(string functionName, T tuple, MsgPackContext context)
            : base(CommandCode.Call, context)
        {
            FunctionName = functionName;
            Tuple = tuple;
            _formatter = context.GetRequiredFormatter<CallRequest<T>>();
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}