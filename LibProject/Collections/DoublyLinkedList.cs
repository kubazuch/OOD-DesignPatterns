namespace BTM.Collections
{
    public class DoublyLinkedList<T> : Collection<T>
    {
        internal class Node
        {
            public T data;
            public Node next;
            public Node prev;

            public Node(T data, Node next = null, Node prev = null)
            {
                this.data = data;
                this.next = next;
                this.prev = prev;
            }
        }

        private Node head;
        private Node tail;

        public class DoublyLinkedListIterator : Collection<T>.Iterator
        {
            internal Node curr;
            protected DoublyLinkedList<T> list;

            public DoublyLinkedListIterator(DoublyLinkedList<T> list)
            {
                this.list = list;
                this.curr = null;
            }

            public override T Current => curr.data;

            public override bool MoveNext()
            {
                if (curr == null)
                {
                    if (list.head == null) return false;
                    curr = list.head;
                    return true;

                }

                if (curr.next == null) return false;
                curr = curr.next;
                return true;
            }

            public override void Reset()
            {
                curr = null;
            }
        }

        class DoublyLinkedListReverseIterator : DoublyLinkedListIterator
        {
            public DoublyLinkedListReverseIterator(DoublyLinkedList<T> list)
                : base(list)
            {
                this.list = list;
                this.curr = null;
            }

            public override bool MoveNext()
            {
                if (curr == null)
                {
                    if (list.tail == null) return false;
                    curr = list.tail;
                    return true;

                }

                if (curr.prev == null) return false;
                curr = curr.prev;
                return true;
            }
        }

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
        }

        public void Add(T val)
        {
            Node newNode = new Node(val, null, tail);

            if (tail != null)
            {
                tail.next = newNode;
            }

            tail = newNode;

            head ??= tail;
        }

        public void Remove(DoublyLinkedListIterator iter)
        {
            Node current = iter.curr;
            if (current.prev == null)
            {
                head = current.next;
                if (head != null)
                {
                    head.prev = null;
                }
                else
                {
                    tail = null;
                }
            }
            else if (current.next == null)
            {
                tail = current.prev;
                tail.next = null;
            }
            else
            {
                current.prev.next = current.next;
                current.next.prev = current.prev;
            }
        }

        public override Iterator GetForwardIterator() =>
            new DoublyLinkedListIterator(this);

        public override Iterator GetReverseIterator() =>
            new DoublyLinkedListReverseIterator(this);
    }
}