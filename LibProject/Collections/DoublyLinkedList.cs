namespace BTM.Collections
{
    public class DoublyLinkedList<T> : Collection<T>
    {
        internal class Node
        {
            public T Data;
            public Node Next;
            public Node Prev;

            public Node(T data, Node next = null, Node prev = null)
            {
                this.Data = data;
                this.Next = next;
                this.Prev = prev;
            }
        }

        private Node _head;
        private Node _tail;

        public class DoublyLinkedListIterator : Collection<T>.Iterator
        {
            internal Node Curr;
            protected DoublyLinkedList<T> List;

            public DoublyLinkedListIterator(DoublyLinkedList<T> list)
            {
                this.List = list;
                this.Curr = null;
            }

            public override T Current => Curr.Data;

            public override bool MoveNext()
            {
                if (Curr == null)
                {
                    if (List._head == null) return false;
                    Curr = List._head;
                    return true;

                }

                if (Curr.Next == null) return false;
                Curr = Curr.Next;
                return true;
            }

            public override void Reset()
            {
                Curr = null;
            }
        }

        class DoublyLinkedListReverseIterator : DoublyLinkedListIterator
        {
            public DoublyLinkedListReverseIterator(DoublyLinkedList<T> list)
                : base(list)
            {
                this.List = list;
                this.Curr = null;
            }

            public override bool MoveNext()
            {
                if (Curr == null)
                {
                    if (List._tail == null) return false;
                    Curr = List._tail;
                    return true;

                }

                if (Curr.Prev == null) return false;
                Curr = Curr.Prev;
                return true;
            }
        }

        public DoublyLinkedList()
        {
            _head = null;
            _tail = null;
        }

        public override void Add(T val)
        {
            Node newNode = new Node(val, null, _tail);

            if (_tail != null)
            {
                _tail.Next = newNode;
            }

            _tail = newNode;

            _head ??= _tail;
        }

        public override void Delete(T obj)
        {
            var iter = (DoublyLinkedListIterator) GetForwardIterator();
            while(iter.MoveNext())
                if (iter.Current.Equals(obj))
                {
                    Remove(iter);
                    return;
                }
        }

        public void Remove(DoublyLinkedListIterator iter)
        {
            Node current = iter.Curr;
            if (current.Prev == null)
            {
                _head = current.Next;
                if (_head != null)
                {
                    _head.Prev = null;
                }
                else
                {
                    _tail = null;
                }
            }
            else if (current.Next == null)
            {
                _tail = current.Prev;
                _tail.Next = null;
            }
            else
            {
                current.Prev.Next = current.Next;
                current.Next.Prev = current.Prev;
            }
        }

        public override Iterator GetForwardIterator() =>
            new DoublyLinkedListIterator(this);

        public override Iterator GetReverseIterator() =>
            new DoublyLinkedListReverseIterator(this);
    }
}