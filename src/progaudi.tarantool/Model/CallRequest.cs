using System;
using ProGaudi.MsgPack.Light;

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

        internal class Formatter : IMsgPackConverter<CallRequest<T>>
        {
            private IMsgPackConverter<uint> _keyConverter;
            private IMsgPackConverter<string> _stringConverter;
            private IMsgPackConverter<T> _tupleConverter;

            public void Initialize(MsgPackContext context)
            {
                _keyConverter = context.GetConverter<uint>();
                _stringConverter = context.GetConverter<string>();
                _tupleConverter = context.GetConverter<T>();
            }

            public void Write(CallRequest<T> value, IMsgPackWriter writer)
            {
                writer.WriteMapHeader(2);

                _keyConverter.Write(Keys.FunctionName, writer);
                _stringConverter.Write(value.FunctionName, writer);

                _keyConverter.Write(Keys.Tuple, writer);
                _tupleConverter.Write(value.Tuple, writer);
            }

            public CallRequest<T> Read(IMsgPackReader reader)
            {
                throw new NotImplementedException();
            }
        }
    }
}
