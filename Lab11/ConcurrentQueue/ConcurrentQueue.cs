namespace ConcurrentQueue;

using LinkedListGenerica;
public class ConcurrentQueue<T>
{
    private LinkedList<T> linkedList;
    public int Count
    {
        get
        {
            lock (obj)
            {
                return linkedList.Count;
            }
        }
    }
    private readonly object obj = new object();

    public ConcurrentQueue()
    {
        linkedList = new LinkedList<T>();
    }

    public bool IsEmpty()
    {
        lock (obj)
        {
            return Count == 0;
        }
    }

    public void Enqueue(T item)
    {
        lock (obj)
        {
            linkedList.Add(item);
        }
    }

    public T Dequeue()
    {
        lock (obj)
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException("Empty queue");
            }
            T item = linkedList.ElementAt(0);
            linkedList.RemoveAt(0);
            return item;
        }
    }

    public T Peek()
    {
        lock(obj)
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException("Empty queue");
            }
            return linkedList.ElementAt(0);
        }
    }
}
