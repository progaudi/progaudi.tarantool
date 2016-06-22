namespace Tarantool.Client.Model.UpdateOperations
{
    public class StringSliceOperation : UpdateOperation<string>
    {
        public int Position { get; }

        public int Offset { get; }

        public StringSliceOperation(
            int fieldNumber,
            int position,
            int offset,
            string argument)
            : base(UpdateOperationType.Splice, fieldNumber, argument)
        {
            Position = position;
            Offset = offset;
        }
    }
}