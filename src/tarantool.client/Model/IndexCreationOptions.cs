namespace Tarantool.Client.Model
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