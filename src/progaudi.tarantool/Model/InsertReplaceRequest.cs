using System;
using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public abstract class InsertReplaceRequest<T> : IRequest
    {
        protected InsertReplaceRequest(CommandCodes code, uint spaceId, T tuple)
        {
            Code = code;
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public uint SpaceId { get; }

        public T Tuple { get; }

        public CommandCodes Code { get; }

        public sealed class Formatter : IMsgPackConverter<InsertReplaceRequest<T>>
        {
            private IMsgPackConverter<uint> _uintConverter;
            private IMsgPackConverter<T> _tupleConverter;

            public void Initialize(MsgPackContext context)
            {
                _uintConverter = context.GetConverter<uint>();
                _tupleConverter = context.GetConverter<T>();
            }

            public void Write(InsertReplaceRequest<T> value, IMsgPackWriter writer)
            {
                writer.WriteMapHeader(2);

                _uintConverter.Write(Keys.SpaceId, writer);
                _uintConverter.Write(value.SpaceId, writer);

                _uintConverter.Write(Keys.Tuple, writer);
                _tupleConverter.Write(value.Tuple, writer);
            }

            public InsertReplaceRequest<T> Read(IMsgPackReader reader)
            {
                throw new NotImplementedException();
            }
        }
    }
}
