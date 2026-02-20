namespace SortedList;

public class SortedList
{
    private LinkedList linkedList;
    public int Count { get { return linkedList.Count; } }

    public SortedList()
    {
        linkedList = new LinkedList();
    }

    public void Add(IComparable item)
    {
        uint index = 0;
        for (uint i = 0; i < linkedList.Count; i++)
        {
            IComparable current = (IComparable)linkedList.ElementAt(i);
            int comparacion = item?.CompareTo(current) ?? (current == null ? 0 : -1);
            if (comparacion < 0)
            {
                index = i;
                linkedList.Insert(index, item);
                return;
            }
        }
        linkedList.Add(item);
    }

    public object ElementAt(uint index)
    {
        if (index >= Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        return linkedList.ElementAt(index);
    }

    public bool Contains(object item)
    {
        return linkedList.Contains(item);
    }

    public bool Remove(object item)
    {
        return linkedList.Remove(item);
    }

    public void RemoveAt(uint index)
    {
        if (index >= Count)
            throw new IndexOutOfRangeException("indice fuera de rango");
        linkedList.RemoveAt(index);
    }

    public void Clear()
    {
        linkedList.Clear();
    }
}
