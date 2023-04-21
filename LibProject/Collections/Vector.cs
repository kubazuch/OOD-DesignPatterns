using System;

namespace BTM.Collections
{
    public class Vector<T> : Collection<T>
    {
        private T[] items;
        private int count;

        public class VectorIterator : Collection<T>.Iterator
        {
            internal int curr;
            protected Vector<T> list;

            public VectorIterator(Vector<T> list)
            {
                this.list = list;
                this.curr = -1;
            }

            public override T Current => list.items[curr];

            public override bool MoveNext()
            {
                if (curr == -1)
                {
                    if (list.count <= 0) return false;
                    curr = 0;
                    return true;

                }

                if (curr >= list.count - 1) return false;
                curr++;
                return true;
            }

            public override void Reset()
            {
                curr = -1;
            }
        }

        public class VectorReverseIterator : VectorIterator
        {
            public VectorReverseIterator(Vector<T> list)
                : base(list)
            {
                this.curr = list.count;
            }

            public override bool MoveNext()
            {
                if (curr == list.count)
                {
                    if (list.count <= 0) return false;
                    curr = list.count - 1;
                    return true;

                }

                if (curr <= 0) return false;
                curr--;
                return true;
            }

            public override void Reset()
            {
                curr = list.count;
            }
        }

        public Vector()
        {
            items = new T[4];
            count = 0;
        }

        public void Add(T item)
        {
            if (count == items.Length)
            {
                Array.Resize(ref items, items.Length * 2);
            }

            items[count] = item;
            count++;
        }

        public void Remove(VectorIterator iter)
        {
            for (int i = iter.curr; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            count--;
            items[count] = default(T);
        }

        public override Iterator GetForwardIterator() =>
            new VectorIterator(this);

        public override Iterator GetReverseIterator() =>
            new VectorReverseIterator(this);

        public T this[int i] => items[i];
    }
}