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
        return new LinkedListEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class LinkedListEnumerator<T> : IEnumerator<T>
    {
        private LinkedList<T> _lista;
        private LinkedList<T>.Node _nodoActual;
        private bool _haComenzado;

        public LinkedListEnumerator(LinkedList<T> lista)
        {
            this._lista = lista;
            this.Reset();
        }

        public T Current
        {
            get
            {
                if (!_haComenzado || _nodoActual == null)
                {
                    throw new InvalidOperationException("Iterador en posición no válida.");
                }
                return _nodoActual.data;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (!_haComenzado)
            {
                _nodoActual = _lista.head;
                _haComenzado = true;
            }
            else if (_nodoActual != null)
            {
                _nodoActual = _nodoActual.next;
            }

            // Si el nodo es distinto de null, hay algo que leer (aunque el dato sea null)
            return _nodoActual != null;
        }

        public void Reset()
        {
            _nodoActual = null;
            _haComenzado = false;
        }

        public void Dispose() { }
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
        while (node != null)
        {
            if (Equals(node.data, item))
                return true;
            node = node.next;
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
        for (int i = 0; i < Count; i++)
        {
            copy.Add((T)ElementAt((uint)i));
        }
        return copy;
    }
}