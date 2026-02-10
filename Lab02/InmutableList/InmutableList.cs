namespace InmutableList;

public class InmutableList
{
    public int Count { private set; get; }
    private LinkedList linkedList;

    public InmutableList(LinkedList linkedList, int count)
    {
        this.linkedList = linkedList;
        this.Count = count;
    }

    public InmutableList Add(object item)
    {
        LinkedList copy = new LinkedList(linkedList);
        copia.Add(x);              
        return new InmutableList(copia);
    }






}
