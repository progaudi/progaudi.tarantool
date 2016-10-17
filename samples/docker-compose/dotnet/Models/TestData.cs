using System.Collections.Generic;

namespace dotnet.Models
{
    public class TestData
    {
        public IReadOnlyList<Dog> AllDogs { get; set;}

        public IReadOnlyList<Dog> DogsOlder5Years { get; set; }
    }
}