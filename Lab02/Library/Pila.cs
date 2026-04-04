
namespace Library;

using InmutableList;

public class Pila
{
    private int capacidad;
    public int Capacidad
    {
        get { return capacidad; }
        private set { capacidad = value; }
    }
    public int Count { private set; get; }
    private InmutableList list;

    public Pila(int capacidad)
    {
        this.capacidad = capacidad;
        list = new InmutableList();
    }

    public void Push(int v)
    {
        if (Count >= Capacidad)
        {
            throw new InvalidOperationException("stack full");
        }
        list.AddFirst(v);       //LIFO
        Count++;
    }

    public int Pop()
    {
        if (Count <= Capacidad)
        {
            throw new InvalidOperationException("empty stack");
        }
        var element = list.ElementAt(0);
        list.RemoveAt(0);
        Count--;
        return (int)element;
    }
}
