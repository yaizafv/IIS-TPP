namespace ConcurrentQueue;

using System.ComponentModel;
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
                throw new InvalidOperationException("Empty queue");
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
                throw new InvalidOperationException("Empty queue");
            }
            // if(TryPeek(out T valor))
            return linkedList.ElementAt(0);
        }
    }

    //para evitar trabajar con excepciones
    bool TryPeek(out T valor)
    {
        lock(obj)
        {
            if(Count == 0)
            {
                valor = default;
                return false;
            }
            valor = linkedList.ElementAt(0);
            return true;
        }
    }
}
