namespace InmutableList;

public class InmutableList
{
    public int Count { private set; get; }
    private LinkedList linkedList;

    public InmutableList() { 
        linkedList = new LinkedList(); 
    }


    public InmutableList(LinkedList linkedList)
    {
        this.linkedList = linkedList;
    }

    public InmutableList Add(object item)
    {
        LinkedList copy = linkedList.Copy();
        copy.Add(item);
        return new InmutableList(copy);
    }

    public object ElementAt(uint index)
    {
        return linkedList.ElementAt(index);
    }

    public InmutableList Set(uint index, object item)
    {
        LinkedList copy = linkedList.Copy();
        copy.Set(index, item);
        return new InmutableList(copy);
    }

    public InmutableList Insert(uint index, object item)
    {
        LinkedList copy = linkedList.Copy();
        copy.Insert(index, item);
        return new InmutableList(copy);
    }

    public bool Contains(object item)
    {
        return linkedList.Contains(item);
    }

    public InmutableList Remove(object item)
    {
        LinkedList copy = linkedList.Copy();
        copy.Remove(item);
        return new InmutableList(copy);
    }

    public InmutableList RemoveAt(uint index)
    {
        LinkedList copy = linkedList.Copy();
        copy.RemoveAt(index);
        return new InmutableList(copy);
    }

    public InmutableList Clear()
    {
        return new InmutableList(new LinkedList());
    }
}
