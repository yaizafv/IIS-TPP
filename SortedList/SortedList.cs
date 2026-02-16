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
        bool found = false;
        for (uint i = 0; i < linkedList.Count; i++)
        {
            IComparable current = (IComparable)linkedList.ElementAt(i);
            if (item.CompareTo(current) < 0)
            {
                index = i;
                found = true;
                break;
            }
        }
        if (found)
        {
            linkedList.Insert(index, item);
        }
        else
        {
            linkedList.Add(item);
        }
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
}
