using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client.Model.Responses
{
    public class DataResponse
    {
        public DataResponse(SqlInfo sqlInfo)
        {
            SqlInfo = sqlInfo;
        }

        public SqlInfo SqlInfo { get; }
    }

    public class DataResponse<T> : DataResponse
    {
        public DataResponse(T data, FieldMetadata[] metadata, SqlInfo sqlInfo)
            : base(sqlInfo)
        {
            Data = data;
            MetaData = metadata;
        }

        public T Data { get; }

        public IReadOnlyList<FieldMetadata> MetaData { get; }
    }
}