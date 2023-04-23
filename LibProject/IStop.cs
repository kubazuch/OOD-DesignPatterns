using System.Collections.Generic;

namespace BTM
{
    public interface IStop : IEntity
    {
        public int Id { get; }
        public List<ILine> Lines { get; }
        public string Name { get; }
        public string Type { get; }
    }
}