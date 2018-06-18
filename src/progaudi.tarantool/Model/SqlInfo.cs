namespace ProGaudi.Tarantool.Client.Model
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