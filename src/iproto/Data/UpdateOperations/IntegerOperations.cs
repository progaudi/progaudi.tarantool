namespace iproto.Data.UpdateOperations
{
    public class AddOperation<T> : UpdateOperation<T>
    {
        public AddOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.Addition, fieldNumber, argument)
        {
        }
    }

    public class SubtractOperation<T> : UpdateOperation<T>
    {
        public SubtractOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.Subtraction, fieldNumber, argument)
        {
        }
    }

    public class BitwiseAndOperation<T> : UpdateOperation<T>
    {
        public BitwiseAndOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.BitwiseAnd, fieldNumber, argument)
        {
        }
    }

    public class BitwiseXorOperation<T> : UpdateOperation<T>
    {
        public BitwiseXorOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.BitwiseXor, fieldNumber, argument)
        {
        }
    }

    public class BitwiseOrOperation<T> : UpdateOperation<T>
    {
        public BitwiseOrOperation(int fieldNumber, T argument)
            : base(UpdateOperationType.BitwiseOr, fieldNumber, argument)
        {
        }
    }
}