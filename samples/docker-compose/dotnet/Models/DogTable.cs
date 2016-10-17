using System.Collections.Generic;

namespace dotnet.Models
{
    public class DogTable
    {
        public DogTable(IReadOnlyList<Dog> dogs, string caption)
        {
            Dogs = dogs;
            Caption = caption;
        }

        public IReadOnlyList<Dog> Dogs { get; }
        public string Caption { get; }
    }
}