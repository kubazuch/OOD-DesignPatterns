using System;
using System.Collections;
using System.Collections.Generic;

namespace BTM.Collections
{
    public abstract class Collection<T> : IEnumerable<T>
    {
        public abstract class Iterator : IEnumerator<T>
        {
            object IEnumerator.Current => Current;
            public abstract T Current { get; }
            public abstract bool MoveNext();
            public abstract void Reset();
            public void Dispose() => GC.SuppressFinalize(this);
        }

        public abstract Iterator GetForwardIterator();
        public abstract Iterator GetReverseIterator();
        public IEnumerator<T> GetEnumerator() => GetForwardIterator();
        IEnumerator IEnumerable.GetEnumerator() => GetReverseIterator();
    }
}