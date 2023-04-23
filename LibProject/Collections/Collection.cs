using System;
using System.Collections;
using System.Collections.Generic;

namespace BTM.Collections
{
    public interface ICollection : IEnumerable
    {
        public interface IIterator : IEnumerator
        {
        }
        public IIterator GetForwardIterator();
        public IIterator GetReverseIterator();
    }

    public abstract class Collection<T> : ICollection, IEnumerable<T>
    {
        public abstract class Iterator : ICollection.IIterator, IEnumerator<T>
        {
            public abstract T Current { get; }
            public abstract bool MoveNext();
            public abstract void Reset();
            object IEnumerator.Current => Current;

            public void Dispose() => GC.SuppressFinalize(this);

        }

        ICollection.IIterator ICollection.GetForwardIterator() => GetForwardIterator();

        ICollection.IIterator ICollection.GetReverseIterator() => GetReverseIterator();

        public abstract Iterator GetForwardIterator();
        public abstract Iterator GetReverseIterator();

        public IEnumerator<T> GetEnumerator() => GetForwardIterator();
        IEnumerator IEnumerable.GetEnumerator() => GetForwardIterator();
    }
}