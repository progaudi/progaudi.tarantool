using MsgPack.Light;

namespace Tarantool.Client.Model.UpdateOperations
{
    public class UpdateOperation<T> : UpdateOperation
    {
        public string OperationType { get; }

        public int FieldNumber { get; }

        public T Argument { get; }

        public UpdateOperation(string operationType, int fieldNumber, T argument)
        {
            OperationType = operationType;
            FieldNumber = fieldNumber;
            Argument = argument;
        }

        public override IMsgPackConverter<UpdateOperation> GetConverter(MsgPackContext context)
        {
            return (IMsgPackConverter<UpdateOperation>) context.GetConverter<UpdateOperation<T>>();
        }
    }

    public abstract class UpdateOperation
    {
        public abstract IMsgPackConverter<UpdateOperation> GetConverter(MsgPackContext context);

        #region Integer Operation Factory

        public static UpdateOperation<byte> CreateAddition(byte argument, int fieldNumber)
        {
            return new UpdateOperation<byte>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<byte> CreateSubtraction(byte argument, int fieldNumber)
        {
            return new UpdateOperation<byte>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<byte> CreateBitwiseAnd(byte argument, int fieldNumber)
        {
            return new UpdateOperation<byte>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<byte> CreateBitwiseXor(byte argument, int fieldNumber)
        {
            return new UpdateOperation<byte>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<byte> CreateBitwiseOr(byte argument, int fieldNumber)
        {
            return new UpdateOperation<byte>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<sbyte> CreateAddition(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation<sbyte>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<sbyte> CreateSubtraction(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation<sbyte>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<sbyte> CreateBitwiseAnd(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation<sbyte>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<sbyte> CreateBitwiseXor(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation<sbyte>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<sbyte> CreateBitwiseOr(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation<sbyte>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<ushort> CreateAddition(ushort argument, int fieldNumber)
        {
            return new UpdateOperation<ushort>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<ushort> CreateSubtraction(ushort argument, int fieldNumber)
        {
            return new UpdateOperation<ushort>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<ushort> CreateBitwiseAnd(ushort argument, int fieldNumber)
        {
            return new UpdateOperation<ushort>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<ushort> CreateBitwiseXor(ushort argument, int fieldNumber)
        {
            return new UpdateOperation<ushort>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<ushort> CreateBitwiseOr(ushort argument, int fieldNumber)
        {
            return new UpdateOperation<ushort>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<short> CreateAddition(short argument, int fieldNumber)
        {
            return new UpdateOperation<short>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<short> CreateSubtraction(short argument, int fieldNumber)
        {
            return new UpdateOperation<short>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<short> CreateBitwiseAnd(short argument, int fieldNumber)
        {
            return new UpdateOperation<short>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<short> CreateBitwiseXor(short argument, int fieldNumber)
        {
            return new UpdateOperation<short>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<short> CreateBitwiseOr(short argument, int fieldNumber)
        {
            return new UpdateOperation<short>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<uint> CreateAddition(uint argument, int fieldNumber)
        {
            return new UpdateOperation<uint>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<uint> CreateSubtraction(uint argument, int fieldNumber)
        {
            return new UpdateOperation<uint>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<uint> CreateBitwiseAnd(uint argument, int fieldNumber)
        {
            return new UpdateOperation<uint>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<uint> CreateBitwiseXor(uint argument, int fieldNumber)
        {
            return new UpdateOperation<uint>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<uint> CreateBitwiseOr(uint argument, int fieldNumber)
        {
            return new UpdateOperation<uint>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<int> CreateAddition(int argument, int fieldNumber)
        {
            return new UpdateOperation<int>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<int> CreateSubtraction(int argument, int fieldNumber)
        {
            return new UpdateOperation<int>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<int> CreateBitwiseAnd(int argument, int fieldNumber)
        {
            return new UpdateOperation<int>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<int> CreateBitwiseXor(int argument, int fieldNumber)
        {
            return new UpdateOperation<int>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<int> CreateBitwiseOr(int argument, int fieldNumber)
        {
            return new UpdateOperation<int>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<ulong> CreateAddition(ulong argument, int fieldNumber)
        {
            return new UpdateOperation<ulong>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<ulong> CreateSubtraction(ulong argument, int fieldNumber)
        {
            return new UpdateOperation<ulong>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<ulong> CreateBitwiseAnd(ulong argument, int fieldNumber)
        {
            return new UpdateOperation<ulong>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<ulong> CreateBitwiseXor(ulong argument, int fieldNumber)
        {
            return new UpdateOperation<ulong>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<ulong> CreateBitwiseOr(ulong argument, int fieldNumber)
        {
            return new UpdateOperation<ulong>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        public static UpdateOperation<long> CreateAddition(long argument, int fieldNumber)
        {
            return new UpdateOperation<long>(UpdateOperationType.Addition, fieldNumber, argument);
        }

        public static UpdateOperation<long> CreateSubtraction(long argument, int fieldNumber)
        {
            return new UpdateOperation<long>(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        public static UpdateOperation<long> CreateBitwiseAnd(long argument, int fieldNumber)
        {
            return new UpdateOperation<long>(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        public static UpdateOperation<long> CreateBitwiseXor(long argument, int fieldNumber)
        {
            return new UpdateOperation<long>(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        public static UpdateOperation<long> CreateBitwiseOr(long argument, int fieldNumber)
        {
            return new UpdateOperation<long>(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        #endregion

        #region Object Operation Factory

        public static UpdateOperation<int> CreateDelete(int fieldNumber, int argument)
        {
            return new UpdateOperation<int>(UpdateOperationType.Delete, fieldNumber, argument);
        }

        public static UpdateOperation<T> CreateInsert<T>(int fieldNumber, T argument)
        {
            return new UpdateOperation<T>(UpdateOperationType.Insert, fieldNumber, argument);
        }

        public static UpdateOperation<T> CreateAssign<T>(int fieldNumber, T argument)
        {
            return new UpdateOperation<T>(UpdateOperationType.Assign, fieldNumber, argument);
        }

        #endregion

        #region String Operation Factory

        public static StringSliceOperation CreateStringSlice(int fieldNumber, int position, int offset, string argument)
        {
            return new StringSliceOperation(fieldNumber, position, offset, argument);
        }

        #endregion
    }
}