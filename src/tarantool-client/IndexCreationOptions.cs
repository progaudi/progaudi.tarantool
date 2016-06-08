namespace tarantool_client
{
    public class IndexCreationOptions
    {
        public IndexCreationOptions(bool unique)
        {
            Unique = unique;
        }

        public bool Unique { get; } 
    }
}