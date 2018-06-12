using System;

namespace ProGaudi.Tarantool.Client.Model
{
    public class DeleteRequest<T> : IRequest
    {
        public DeleteRequest(uint spaceId, uint indexId, T key)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public T Key { get; }

        public CommandCodes Code => CommandCodes.Delete;

        //internal class DeletePacketConverter : IMsgPackConverter<DeleteRequest<T>>
        //{
        //    private IMsgPackConverter<Keys> _keyConverter;
        //    private IMsgPackConverter<uint> _uintConverter;
        //    private IMsgPackConverter<T> _selectKeyConverter;

        //    public void Initialize(MsgPackContext context)
        //    {
        //        _keyConverter = context.GetConverter<Keys>();
        //        _uintConverter = context.GetConverter<uint>();
        //        _selectKeyConverter = context.GetConverter<T>();
        //    }

        //    public void Write(DeleteRequest<T> value, IMsgPackWriter writer)
        //    {
        //        writer.WriteMapHeader(3);

        //        _keyConverter.Write(Key.SpaceId, writer);
        //        _uintConverter.Write(value.SpaceId, writer);

        //        _keyConverter.Write(Key.IndexId, writer);
        //        _uintConverter.Write(value.IndexId, writer);

        //        _keyConverter.Write(Key.Key, writer);
        //        _selectKeyConverter.Write(value.Key, writer);
        //    }

        //    public DeleteRequest<T> Read(IMsgPackReader reader)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}