using System;
using System.Collections;
using ICollection = BTM.Collections.ICollection;

namespace ConsoleProject.CLI.Arguments
{
    public class TypeArgument : CommandArgument<ICollection>
    {
        private readonly DataManager _data;

        public TypeArgument(DataManager data, bool required = false, string name = "name_of_the_class") 
            : base(name, required)
        {
            _data = data;
        }

        public override ICollection Parse(string arg)
        {
            if (!_data.Mapping.TryGetValue(arg, out var collection))
                throw new ArgumentException($"Unknown type: `§l{arg}§r`. Possible types: §l{string.Join(", ", _data.Mapping.Keys)}");

            return new NamedCollection(arg, collection);
        }
    }

    public class NamedCollection : ICollection 
    {
        public string Name { get; }
        private readonly ICollection _collection;

        public NamedCollection(string name, ICollection collection)
        {
            Name = name;
            _collection = collection;
        }

        public IEnumerator GetEnumerator() => _collection.GetEnumerator();

        public ICollection.IIterator GetForwardIterator() => _collection.GetForwardIterator();

        public ICollection.IIterator GetReverseIterator() => _collection.GetReverseIterator();

        public void Add(object obj) => _collection.Add(obj);

        public void Delete(object obj) => _collection.Delete(obj);
    }
}
