using System;
using System.Collections.Generic;

namespace BTM.Collections
{
    public static class Algorithms
    {
        public static object Find(ICollection collection, Predicate<object> predicate, bool direction = true)
        {
            ICollection.IIterator iterator = direction ? collection.GetForwardIterator() : collection.GetReverseIterator();

            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    return iterator.Current;

            return null;
        }

        public static void Print(ICollection collection, Predicate<object> predicate, bool direction = true)
        {
            ICollection.IIterator iterator = direction ? collection.GetForwardIterator() : collection.GetReverseIterator();

            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    Console.WriteLine(iterator.Current);
        }

        /***************************************************************************************************/

        public static object Find(ICollection.IIterator iterator, Predicate<object> predicate)
        {
            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    return iterator.Current;

            return null;
        }

        public static void ForEach(ICollection.IIterator iterator, Action<object> function)
        {
            while (iterator.MoveNext())
                function(iterator.Current);
        }

        public static int CountIf(ICollection.IIterator iterator, Predicate<object> predicate)
        {
            int count = 0;
            while (iterator.MoveNext())
                if (predicate(iterator.Current))
                    count++;

            return count;
        }
    }
}
