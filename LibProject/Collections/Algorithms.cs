using System;
using System.Collections.Generic;

namespace BTM.Collections
{
    public static class Algorithms
    {
        public static object Find<T>(Collection<T> collection, Predicate<T> predicate, bool direction = true)
        {
            Collection<T>.Iterator iterator = direction ? collection.GetForwardIterator() : collection.GetReverseIterator();

            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    return iterator.Current;

            return null;
        }

        public static void Print<T>(Collection<T> collection, Predicate<T> predicate, bool direction = true)
        {
            Collection<T>.Iterator iterator = direction ? collection.GetForwardIterator() : collection.GetReverseIterator();

            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    Console.WriteLine(iterator.Current);
        }

        /***************************************************************************************************/

        public static object Find<T>(Collection<T>.Iterator iterator, Predicate<T> predicate)
        {
            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    return iterator.Current;

            return null;
        }

        public static void ForEach<T>(Collection<T>.Iterator iterator, Action<T> function)
        {
            while (iterator.MoveNext())
                function(iterator.Current);
        }

        public static int CountIf<T>(Collection<T>.Iterator iterator, Predicate<T> predicate)
        {
            int count = 0;
            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    count++;

            return count;
        }
    }
}
