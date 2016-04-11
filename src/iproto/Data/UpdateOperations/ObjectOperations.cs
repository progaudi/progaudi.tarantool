namespace iproto.Data.UpdateOperations
{
    public class AssignOperation<T> : UpdateOperation<T>
    {
        public AssignOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.Assign, fieldNumber, argument)
        {
        }
    }

    public class InsertOperation<T> : UpdateOperation<T>
    {
        public InsertOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.Insert, fieldNumber, argument)
        {
        }
    }
}