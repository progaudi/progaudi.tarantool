using Tarantool.Client.Model;

namespace dotnet.Models
{
    public class Dog
    {
        public Dog(Tuple<long, string, long> tuple)
        {
            Id = tuple.Item1;
            Name = tuple.Item2;
            Age = tuple.Item3;
        }

        public long Id { get; }

        public string Name { get; }

        public long Age { get; }
    }
}