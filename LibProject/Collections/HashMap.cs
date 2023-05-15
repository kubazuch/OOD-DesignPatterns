using System;

namespace BTM.Collections
{
    public class HashMap<T> : Collection<T>
    {
        public class HashMapIterator : Iterator
        {
            internal int Pos;
            protected HashMap<T> Map;

            internal HashMapIterator(HashMap<T> map)
            {
                this.Map = map;
                Pos = -1;
            }

            public override T Current => Map.Items[Pos];

            public override bool MoveNext()
            {
                if (Pos == -1)
                {
                    if (Map.Capacity <= 0) return false;
                    Pos = 0;
                }

                Pos++;
                while (Pos < Map.Capacity && !Map._filled[Pos])
                    Pos++;

                return Pos < Map.Capacity;
            }

            public override void Reset()
            {
                Pos = -1;
            }
        }

        public class HashMapReverseIterator : HashMapIterator
        {
            internal HashMapReverseIterator(HashMap<T> map)
                : base(map)
            {
                this.Map = map;
                Pos = map.Capacity;
            }

            public override bool MoveNext()
            {
                if (Pos == Map.Capacity)
                {
                    if (Map.Capacity <= 0) return false;
                    Pos = Map.Capacity - 1;
                }

                Pos--;
                while (Pos >= 0 && !Map._filled[Pos])
                    Pos--;

                return Pos >= 0;
            }

            public override void Reset()
            {
                Pos = Map.Capacity;
            }
        }

        public int Capacity { get; }
        public T[] Items { get; }
        private readonly bool[] _filled;

        public HashMap(int capacity = 100)
        {
            Capacity = capacity;
            Items = new T[capacity];
            _filled = new bool[capacity];
        }

        public override void Add(T val)
        {
            int pos = val.GetHashCode() % Capacity;
            if (_filled[pos]) throw new ArgumentException("Hash collision!");

            Items[pos] = val;
            _filled[pos] = true;
        }

        public override void Delete(T obj)
        {
            var iter = (HashMapIterator) GetForwardIterator();
            while (iter.MoveNext())
                if (iter.Current.Equals(obj))
                {
                    Remove(iter);
                    return;
                }
        }

        public void Remove(HashMapIterator iterator)
        {
            Items[iterator.Pos] = default;
            _filled[iterator.Pos] = false;
        }

        public override Iterator GetForwardIterator() => new HashMapIterator(this);

        public override Iterator GetReverseIterator() => new HashMapReverseIterator(this);
    }
}
