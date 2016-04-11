namespace iproto.Data.UpdateOperations
{
    public class UpdateOperationType
    {
        //Works only with integer
        public static string Addition = "+";
        public static string Subtraction = "-";
        public static string BitwiseAnd = "&";
        public static string BitwiseXor = "^";
        public static string BitwiseOr = "|";

        //Works on any field
        public static string Delete = "#";
        public static string Insert = "!";
        public static string Assign = "=";

        //Works only for string
        public static string Splice = ":";
    }
}