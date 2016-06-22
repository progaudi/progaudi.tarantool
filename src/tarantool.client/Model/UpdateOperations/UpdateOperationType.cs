namespace Tarantool.Client.Model.UpdateOperations
{
    public static class UpdateOperationType
    {
        //Works only with integer
        public static readonly string Addition = "+";

        public static readonly string Subtraction = "-";

        public static readonly string BitwiseAnd = "&";

        public static readonly string BitwiseXor = "^";

        public static readonly string BitwiseOr = "|";

        //Works on any field
        public static readonly string Delete = "#";

        public static readonly string Insert = "!";

        public static readonly string Assign = "=";

        //Works only for string
        public static readonly string Splice = ":";
    }
}