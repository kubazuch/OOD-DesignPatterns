using System;

namespace BTM.Collections
{
    public class Vector<T> : Collection<T>
    {
        private T[] _items;
        private int _count;

        public class VectorIterator : Collection<T>.Iterator
        {
            internal int Curr;
            protected Vector<T> List;

            public VectorIterator(Vector<T> list)
            {
                this.List = list;
                this.Curr = -1;
            }

            public override T Current => List._items[Curr];

            public override bool MoveNext()
            {
                if (Curr == -1)
                {
                    if (List._count <= 0) return false;
                    Curr = 0;
                    return true;

                }

                if (Curr >= List._count - 1) return false;
                Curr++;
                return true;
            }

            public override void Reset()
            {
                Curr = -1;
            }
        }

        public class VectorReverseIterator : VectorIterator
        {
            public VectorReverseIterator(Vector<T> list)
                : base(list)
            {
                this.Curr = list._count;
            }

            public override bool MoveNext()
            {
                if (Curr == List._count)
                {
                    if (List._count <= 0) return false;
                    Curr = List._count - 1;
                    return true;

                }

                if (Curr <= 0) return false;
                Curr--;
                return true;
            }

            public override void Reset()
            {
                Curr = List._count;
            }
        }

        public Vector()
        {
            _items = new T[4];
            _count = 0;
        }

        public override void Add(T item)
        {
            if (_count == _items.Length)
            {
                Array.Resize(ref _items, _items.Length * 2);
            }

            _items[_count] = item;
            _count++;
        }

        public void Remove(VectorIterator iter)
        {
            for (int i = iter.Curr; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _count--;
            _items[_count] = default(T);
        }

        public override Iterator GetForwardIterator() =>
            new VectorIterator(this);

        public override Iterator GetReverseIterator() =>
            new VectorReverseIterator(this);

        public T this[int i] => _items[i];
    }
}