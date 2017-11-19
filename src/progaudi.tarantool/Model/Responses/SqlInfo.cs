namespace ProGaudi.Tarantool.Client.Model.Responses
{
    public class SqlInfo
    {
        public SqlInfo(int rowCount)
        {
            RowCount = rowCount;
        }

        public int RowCount { get; }
    }
}