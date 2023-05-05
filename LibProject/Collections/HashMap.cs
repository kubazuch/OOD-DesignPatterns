using System;

namespace BTM.Collections
{
    public class HashMap<T> : Collection<T>
    {
        public class HashMapIterator : Iterator
        {
            internal int pos;
            protected HashMap<T> map;

            internal HashMapIterator(HashMap<T> map)
            {
                this.map = map;
                pos = -1;
            }

            public override T Current => map.Items[pos];

            public override bool MoveNext()
            {
                if (pos == -1)
                {
                    if (map.Capacity <= 0) return false;
                    pos = 0;
                }

                pos++;
                while (pos < map.Capacity && !map._filled[pos])
                    pos++;

                return pos < map.Capacity;
            }

            public override void Reset()
            {
                pos = -1;
            }
        }

        public class HashMapReverseIterator : HashMapIterator
        {
            internal HashMapReverseIterator(HashMap<T> map)
                : base(map)
            {
                this.map = map;
                pos = map.Capacity;
            }

            public override bool MoveNext()
            {
                if (pos == map.Capacity)
                {
                    if (map.Capacity <= 0) return false;
                    pos = map.Capacity - 1;
                }

                pos--;
                while (pos >= 0 && !map._filled[pos])
                    pos--;

                return pos >= 0;
            }

            public override void Reset()
            {
                pos = map.Capacity;
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

        public void Add(T val)
        {
            int pos = val.GetHashCode() % Capacity;
            if (_filled[pos]) throw new ArgumentException("Hash collision!");

            Items[pos] = val;
            _filled[pos] = true;
        }

        public void Remove(HashMapIterator iterator)
        {
            Items[iterator.pos] = default;
            _filled[iterator.pos] = false;
        }

        public override Iterator GetForwardIterator() => new HashMapIterator(this);

        public override Iterator GetReverseIterator() => new HashMapReverseIterator(this);
    }
}
