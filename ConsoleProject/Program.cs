using System;
using System.Collections.Generic;
using System.Linq;
using BTM;
using BTM.Collections;

namespace ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataManager manager = new DataManager(DataRepresentation.TUPLE_STACK);

            DoublyLinkedList<string> list = new DoublyLinkedList<string>();
            list.Add("kuzu");
            list.Add("marcin");
            list.Add("zosia");
            list.Add("szymon");
            list.Add("konrad");
            list.Add("arek");
            var iterator = (DoublyLinkedList<string>.DoublyLinkedListIterator)list.GetForwardIterator();
            Algorithms.Find(iterator, s => s == "kuzu");
            list.Remove(iterator);

            Console.WriteLine("LinkedList Forward:");
            var iter = list.GetForwardIterator();
            while (iter.MoveNext())
            {
                Console.WriteLine(iter.Current);
            }

            Console.WriteLine("\nLinkedList Reversed:");
            var iter2 = list.GetReverseIterator();
            while (iter2.MoveNext())
            {
                Console.WriteLine(iter2.Current);
            }

            Vector<string> list2 = new Vector<string>();
            list2.Add("kuzu");
            list2.Add("marcin");
            list2.Add("zosia");
            list2.Add("szymon");
            list2.Add("szarcin");
            list2.Add("szosia");
            list2.Add("szuzu");
            list2.Add("arek");
            list2.Add("konrad");

            var itera2 = (Vector<string>.VectorIterator)list2.GetForwardIterator();
            Algorithms.Find(itera2, s => s == "kuzu");
            list2.Remove(itera2);

            Console.WriteLine("\nVector Forward:");
            iter = list2.GetForwardIterator();
            while (iter.MoveNext())
            {
                Console.WriteLine(iter.Current);
            }
            Console.WriteLine("\nVector Reversed:");
            iter2 = list2.GetReverseIterator();
            while (iter2.MoveNext())
            {
                Console.WriteLine(iter2.Current);
            }

            Console.WriteLine("\nFirst in vector like sz*:");
            Console.WriteLine(Algorithms.Find(list2, str => str.StartsWith("sz"), true));
            Console.WriteLine("\nLast in vector like sz*:");
            Console.WriteLine(Algorithms.Find(list2, str => str.StartsWith("sz"), false));

            Console.WriteLine("\nAll in list with a, from beginning:");
            Algorithms.Print(list, str => str.Contains("a"), true);
            Console.WriteLine("\nAll in list with a, from end:");
            Algorithms.Print(list, str => str.Contains("a"), false);
        }
    }
}
