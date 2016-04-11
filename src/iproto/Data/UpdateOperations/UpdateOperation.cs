namespace iproto.Data.UpdateOperations
{
    public abstract class UpdateOperation<T>
    {
        public string OperationType { get; }

        public int FieldNumber { get; }

        public T Argument { get; }

        protected UpdateOperation(string operationType, int fieldNumber, T argument)
        {
            OperationType = operationType;
            FieldNumber = fieldNumber;
            Argument = argument;
        }
    }
}