using System.Collections;

namespace LinkedListIterableGenerica;

public class LinkedList<T> : IEnumerable<T>
{
    public class Node
    {
        public T data;
        public Node next;

        public Node(T data)
        {
            this.data = data;
        }
    }

    private Node head;
    public int Count { private set; get; }

    public IEnumerator<T> GetEnumerator()
    {
        return new LinkedListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class LinkedListEnumerator : IEnumerator<T>
    {
        private LinkedList<T> list;
        private LinkedList<T>.Node actual;
        private bool hasStarted;

        public LinkedListEnumerator(LinkedList<T> list)
        {
            this.list = list;
            this.Reset();
        }

        public T Current
        {
            get
            {
                if (!hasStarted || actual == null)
                {
                    throw new IndexOutOfRangeException("Iterador en posición no valida");
                }
                return actual.data;
            }
        }

        object IEnumerator.Current { get { return Current; } }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (!hasStarted)
            {
                actual = list.head;
                hasStarted = true;
            }
            else if (actual != null)
            {
                actual = actual.next;
            }
            return actual != null;
        }

        public void Reset()
        {
            actual = null;
            hasStarted = false;
        }
    }

    public void Add(T item)
    {
        Node nuevo = new Node(item);
        if (head == null)
            head = nuevo;
        else
        {
            Node actual = head;
            while (actual.next != null)
            {
                actual = actual.next;
            }
            actual.next = nuevo;
        }
        Count++;
    }

    public object ElementAt(uint index)     //uint no admite negativos
    {
        if (index >= Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        Node actual = head;
        for (int i = 0; i < index; i++)
        {
            actual = actual.next;
        }
        return actual.data;
    }

    public void Set(uint index, T item)
    {
        if (index >= Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        Node actual = head;
        for (int i = 0; i < index; i++)
            actual = actual.next;

        actual.data = item;
    }

    public void Insert(uint index, T item)
    {
        if (index > Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        Node nuevo = new Node(item);
        if (index == 0)
        {
            nuevo.next = head;
            head = nuevo;
        }
        else
        {
            Node actual = head;
            for (int i = 0; i < index - 1; i++)
            {
                actual = actual.next;
            }
            nuevo.next = actual.next;
            actual.next = nuevo;
        }
        Count++;
    }

    public bool Contains(T item)
    {
        Node node = head;
        foreach (T element in this)
        {
            if (Equals(element, item)) return true;
        }
        return false;
    }


    public bool Remove(T item)
    {
        if (Count == 0)
            return false;
        if (Equals(head.data, item))
        {
            head = head.next;
            Count--;
            return true;
        }
        Node actual = head;
        while (actual.next != null)
        {
            if (Equals(actual.next.data, item))
            {
                actual.next = actual.next.next;
                Count--;
                return true;
            }
            actual = actual.next;
        }
        return false;
    }

    public void RemoveAt(uint index)
    {
        if (index >= Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        if (index == 0)
            head = head.next;
        else
        {
            Node actual = head;
            for (int i = 0; i < index - 1; i++)
            {
                actual = actual.next;
            }
            actual.next = actual.next.next;
        }
        Count--;
    }

    public void Clear()
    {
        head = null;
        Count = 0;
    }

    public LinkedList<T> Copy()
    {
        LinkedList<T> copy = new LinkedList<T>();
        foreach(T element in this)
        {
            copy.Add(element);
        }
        return copy;
    }
}