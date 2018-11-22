using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class EvalRequest<T> : Request
    {
        private readonly IMsgPackFormatter<EvalRequest<T>> _formatter;

        public EvalRequest(string expression, T tuple, MsgPackContext context)
            : base(CommandCode.Eval, context)
        {
            Expression = expression;
            Tuple = tuple;
            _formatter = context.GetRequiredFormatter<EvalRequest<T>>();
        }

        public string Expression { get; }

        public T Tuple { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}