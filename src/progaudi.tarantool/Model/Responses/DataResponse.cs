using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client.Model.Responses
{
    public class DataResponse<T>
    {
        public DataResponse(T data, FieldMetadata[] metadata)
        {
            Data = data;
            MetaData = metadata;
        }

        public T Data { get; }

        public IReadOnlyList<FieldMetadata> MetaData { get; }
    }
}