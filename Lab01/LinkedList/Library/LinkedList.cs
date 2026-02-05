namespace Library;

public class LinkedList
{
    public class Node
    {
        public object data;
        public Node next;

        public Node(object data)
        {
            this.data = data;
        }
    }

    private Node head;
    public int Count {private set; get;}
    public void Add(object item)
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

    public object ElementAt(int index)
    {
        IndexValidation(index);
        Node actual = head;
        for (int i = 0; i < index; i++)
        {
            actual = actual.next;
        }
        return actual.data;
    }

    public void Set(int index, object item)
    {
        IndexValidation(index);
        Node actual = head;
        for (int i = 0; i < index; i++)
            actual = actual.next;

        actual.data = item;
    }

    public void Insert(int index, object item)
    {
        if (index < 0 || index > Count)
            throw new IndexOutOfRangeException();
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

    public bool Contains(object item)
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


    public bool Remove(object item)
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

    public void RemoveAt(int index)
    {
        IndexValidation(index);
        if (index == 0)
            head = head.next;
        else
        {
            Node actual = head!;
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

    private void IndexValidation(int index)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException();
    }

}
